using System;
using System.Collections;
using System.Resources;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Audio;

public class PlayerController : NetworkBehaviour
{
    public static event Action<bool> OnIsAim;
    public static event Action<bool> SetSkillUI;
    public static event Action<Transform> OnLocalPlayerSpawned;         //플레이어 생성 이벤트


    protected CharacterInfo characterInfo;              //캐릭터 정보
    private CharacterController controller;             //캐릭터 컨트롤러 ->씬 내에서 플레이어 오브젝트의 자식 프리팹으로 올 캐릭터에 달려있다.
    protected Animator animator;                          //애니메이터 역시   씬 내에서 플레이어 오브젝트의 자식 프리팹으로 올 캐릭터에 달려있다.
    public Transform target;                            //sfx 소리 재생 위치
    public MultiAimConstraint multiAimciConstraint;     //상체 뒤틀림 방지?

    private Vector3 moveDirection;                      //이동 방향
    private Vector3 cameraForward;                      //
    private Vector3 cameraRight;                        //

    protected string atkType;                           //공격타입
    protected string dfnType;                           //방어 타입
    public string currentAnimation;                     //현재 애니메이션 상태

    private float jumpForce = 2.0f;                     //점프 강도
    private float verticalVelocity = 0f;                //
    public float moveSpeed = 5f;                        //이동 속도
    public float gravity = 9.8f;                        //중력
    public float animationSpeed = 3.0f;                 //애니메이션 재생 속도

    protected bool isAim = false;                       //aim 상태인가? -> GunController에서도 사용
    public bool isSkillPlaying = false;              //스킬 애니메이션이 재생중인가? -> 스킬 애니메이션 동안은 공격 불가능
    private bool isMoving = false;                      //움직이고 있는가?
    private bool isJump = false;                        //현재 점프 상태인가?
    private bool isSkillCool = false;                   //스킬 쿨타임인가? -> true일 경우 쿨타임 상태


    private string characterName;

    private GamePlayNetworkSyncManager networkSync;

    private void OnEnable()
    {
        CharacterSpawnManager.OnLoadCharacterData += SetCharacterData;
    }
    private void OnDisable()
    {
        CharacterSpawnManager.OnLoadCharacterData -= SetCharacterData;
    }

    //네트워크 매니저를 통해 플레이어 생성 확인
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        if (IsOwner)
        {
            OnLocalPlayerSpawned?.Invoke(transform);
            RequestCharacterDataServerRpc(gameObject.name);
        }
    }

    [ServerRpc]
    private void RequestCharacterDataServerRpc(string characterName, ServerRpcParams rpcParams = default)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("JsonData/characterData");
        if (jsonFile == null)
        {
            Debug.LogError("캐릭터 데이터 파일 없음.");
            return;
        }

        CharacterData characterData = JsonUtility.FromJson<CharacterData>(jsonFile.text);
        if (characterData == null || characterData.characters == null)
        {
            Debug.LogError("캐릭터 데이터 로딩 실패.");
            return;
        }

        foreach (CharacterInfo info in characterData.characters)
        {
            if (info.name == characterName.Replace("(Clone)", ""))
            {
                string json = JsonUtility.ToJson(info);
                ReceiveCharacterDataClientRpc(json, rpcParams.Receive.SenderClientId);
                return;
            }
        }

        Debug.LogError($"캐릭터 {characterName} 정보 없음.");
    }



    [ClientRpc]
    private void ReceiveCharacterDataClientRpc(string json, ulong targetClientId)
    {
        if (NetworkManager.Singleton.LocalClientId != targetClientId) return;

        CharacterInfo info = JsonUtility.FromJson<CharacterInfo>(json);
        SetCharacterData(info);
    }

    public void SetCharacterData(CharacterInfo info)
    {
        this.characterInfo = info;
        characterName = info.name;
        Debug.Log("playerController  " + info.maxAmmo);

        GunController gunController = GetComponent<GunController>();
        if (gunController != null)
        {
            gunController.ReceiveCharacterData(info);
        }
    }


    void Start()
    {
        Debug.Log("아아아아아아아아아아아아");
        //자식 노드에서 가져오기, 캐릭터 선택을 고려하면 플레이어는 빈 오브젝트고 거기로 선택한 캐릭터를 자식으로 불러오는게 하기 25.03.06
        controller = GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        //소리가 나는 위치, 구분하기 쉽게 하기 위해 이름 변경이 필요해 보임(2025-04-04)
        target = GameObject.Find("target").GetComponentInChildren<Transform>();

        networkSync = GetComponent <GamePlayNetworkSyncManager>();
    }

    void Update()
    {
        if (!IsOwner) return;

        if (isSkillPlaying)
        {
            MoveMent();
            return;
        }

        MoveMent();     //이동
        Aim();          //조준
        Skill();        //스킬
        Ult();          //궁극기
    }

    public void MoveMent()
    {
        // 입력 받기
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 이동 벡터 설정
        Vector3 move = new Vector3(horizontal, 0, vertical).normalized;

        // 현재 카메라의 회전 값 가져오기
        cameraForward = Camera.main.transform.forward;
        cameraRight = Camera.main.transform.right;
        // Y축 방향 제거 (수직 이동 방지)
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        // 정규화 (길이를 1로 조정)
        cameraForward.Normalize();
        cameraRight.Normalize();

        // speed 값 즉각 반영 (키를 누르는 즉시 애니메이션 전환됨)
        float speed = move.magnitude;

        isMoving = move.magnitude > 0;
        animator.SetBool("isMoving", isMoving);

        //Vector3 moveXZ = (cameraForward * vertical + cameraRight * horizontal) * moveSpeed;

        //// **이전 Y값 유지!**
        //moveDirection.x = moveXZ.x;
        //moveDirection.z = moveXZ.z;

        // 중력 적용
        if (!controller.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
            isJump = false;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //점프는 여기에 구현
                Debug.Log("check");
                verticalVelocity = jumpForce;
                isJump = true;
            }

            moveDirection.y = 0f;
        }

        //moveDirection = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);

        // 입력 값을 카메라 기준으로 변환
        moveDirection = (cameraForward * vertical + cameraRight * horizontal);

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        // 이동 처리
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        //SendPositionServerRpc(transform.position);
        //controller.Move(moveDirection * Time.deltaTime);
        // **Player 오브젝트의 회전값 강제 고정 (X=90 유지)**
        //transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        //// **캐릭터의 실제 모델만 회전 (Y축만 회전)**
        //if (move.magnitude > 0)
        //{
        //    Transform characterTransform = controller.transform; // CharacterController가 있는 오브젝트
        //    Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
        //    characterTransform.rotation = Quaternion.Slerp(characterTransform.rotation, newRotation, Time.deltaTime * 10f);
        //}
    }

    public void Aim()
    {
        //조준시 이동 제어를 위해,우선 조준에 대한 코드는 GunController대신 여기서 처리

        if (Input.GetMouseButton(1))
        {
            isAim = true;
            OnIsAim?.Invoke(isAim);
        }
        else
        {
            isAim = false;
            OnIsAim?.Invoke(isAim);
        }

        if (isAim) 
        {
            //조준시에는 이동할 때 보다는 빨리 회전 2025-03-10 23:16
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 7f);
        }
        else if (moveDirection != Vector3.zero)// 이동 중이면 이동 방향으로 캐릭터 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            float rotationSpeed = 300f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void Skill()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isSkillCool)
        {
            StartCoroutine(UseSkill());
        }
    }

    public void Ult()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //if 조건에 추가로 궁극기가 찾는지 확인 필요
        }
    }

    public IEnumerator UseSkill()
    {
        isSkillCool = true;
        isSkillPlaying = true;
        SetSkillUI?.Invoke(true);

        characterName.Replace("(Clone)", "");
        string skillName = characterName + "Skill";


        //애니메이션은 Rpc에서 제어
        if (IsOwner)
        {
            networkSync.TriggerSkillRpc(skillName);
        }



        bool isSkillState = false;

        while (!isSkillState)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Skill"))
            {
                isSkillState = true;
                break;
            }
            yield return null; // 다음 프레임까지 대기
        }

        // 애니메이션 길이만큼 대기
        float animTime = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animTime);

        isSkillPlaying = false;

        float tempSkillCool = 10.0f;

        yield return new WaitForSeconds(tempSkillCool);
        isSkillCool = false;
    }
}
