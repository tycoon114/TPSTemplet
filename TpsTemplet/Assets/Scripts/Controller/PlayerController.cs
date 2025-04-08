using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{

    public static event Action<bool> OnIsAim;

    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;

    public float moveSpeed = 5f;   // 이동 속도
    public float gravity = 9.8f;   // 중력
    private float jumpForce = 2.0f;


    protected bool isAim = false;
    protected int atkType;          //공격타입
    protected int dfnType;          //방어 타입

    public float animationSpeed = 3.0f; //애니메이션 재생 속도
    public string currentAnimation;

    public Transform target; // sfx 소리 재생 위치
    public MultiAimConstraint multiAimciConstraint;         //상체 뒤틀림 방지?

    protected CharacterInfo characterInfo;

    private bool isMoving = false;
    private bool isJump = false;    //현재 점프 상태인지

    private float verticalVelocity = 0f;

    private void OnEnable()
    {
        CharacterSpawnManager.OnLoadCharacterData += SetCharacterData;
    }
    private void OnDisable()
    {
        CharacterSpawnManager.OnLoadCharacterData -= SetCharacterData;
    }

    //spawnManager에서 가져온 캐릭터 데이터 받기
    private void SetCharacterData(CharacterInfo info)
    {
        this.characterInfo = info;
    }

    void Start()
    {
        //자식 노드에서 가져오기, 캐릭터 선택을 고려하면 플레이어는 빈 오브젝트고 거기로 선택한 캐릭터를 자식으로 불러오는게 하기 25.03.06
        controller = GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        //소리가 나는 위치, 구분하기 쉽게 하기 위해 이름 변경이 필요해 보임(2025-04-04)
        target = GameObject.Find("target").GetComponentInChildren<Transform>();
    }

    void Update()
    {
        // 입력 받기
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 이동 벡터 설정
        Vector3 move = new Vector3(horizontal, 0, vertical).normalized;

        // 현재 카메라의 회전 값 가져오기
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
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

        //if (isMoving && controller.isGrounded && !isPlayingFootsteps)
        //{
        //    StartCoroutine(PlayFootsteps());
        //}

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


        if (Input.GetKeyDown(KeyCode.E))
        {
            //if 조건에 추가로 쿨타임이 완료됬는지 확인 필요
            //스킬 버튼 비활성화
            //스킬 버튼 알파값 낮추기 -? 이벤트를 걸어서 플레이어 매니저에서 구현...

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //if 조건에 추가로 궁극기가 찾는지 확인 필요

        }


    }
}
