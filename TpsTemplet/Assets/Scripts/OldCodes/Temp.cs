using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Temp : MonoBehaviour
{
    //public Transform playerBody; // 캐릭터 본체
    //public Transform gunModel; // 총기 모델
    //private Transform muzzlePoint; // 총구 위치
    //public Camera mainCamera; // 메인 카메라
    //private bool isAiming = false; // 조준 여부

    //void Start()
    //{
    //    // 🔍 fire_01을 찾기 (깊숙한 곳까지 탐색)
    //    muzzlePoint = GetMuzzlePoint();
    //}

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(1)) // 우클릭(조준 시작)
    //    {
    //        isAiming = true;
    //    }
    //    if (Input.GetMouseButtonUp(1)) // 우클릭 해제
    //    {
    //        isAiming = false;
    //    }

    //    if (isAiming)
    //    {
    //        AimAtMouse();
    //    }
    //}

    //void AimAtMouse()
    //{
    //    // 🔍 마우스 위치에서 3D 월드 좌표 얻기
    //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    Vector3 aimTarget;
    //    if (Physics.Raycast(ray, out hit)) // 마우스가 어떤 오브젝트에 닿으면
    //    {
    //        aimTarget = hit.point; // 해당 위치를 조준
    //    }
    //    else
    //    {
    //        aimTarget = ray.origin + ray.direction * 50f; // 아무것도 없으면 먼 거리 조준
    //    }

    //    // 🔄 캐릭터 회전 (조준 방향을 바라보게)
    //    Vector3 lookDirection = (aimTarget - playerBody.position).normalized;
    //    lookDirection.y = 0; // Y축 회전 방지 (수평 회전만)
    //    playerBody.forward = lookDirection;

    //    // 🔫 총구 회전 (마우스를 바라보게)
    //    if (muzzlePoint != null)
    //    {
    //        muzzlePoint.LookAt(aimTarget);
    //    }
    //}

    //Transform GetMuzzlePoint()
    //{
    //    // 🔍 깊숙한 곳에서도 fire_01 찾기
    //    Transform[] allChildren = gunModel.GetComponentsInChildren<Transform>();
    //    foreach (Transform child in allChildren)
    //    {
    //        if (child.name == "fire_01")
    //        {
    //            return child;
    //        }
    //    }
    //    Debug.LogError("fire_01을 찾을 수 없습니다!");
    //    return null;
    //}
}



//    private Transform playerTransform; // 플레이어 Transform
//    private Transform weaponTransform; // 무기 (Bip001_Weapon)
//    private Transform spineTransform;  // 상체 (Bip001 Spine)

//    public LayerMask aimLayerMask; // 조준 가능한 레이어 (지면, 벽, 적 등)

//    public float maxSpineRotationX = 30f; // 상체 X축 최대 회전각
//    public float maxSpineRotationY = 50f; // 상체 Y축 최대 회전각
//    public float rotationSpeed = 10f;     // 회전 속도

//    private bool isAiming = false;

//    void Start()
//{
//    playerTransform = GetComponentInParent<PlayerController2>()?.transform;
//    if (playerTransform == null)
//    {
//        Debug.LogError("PlayerController를 찾을 수 없습니다!");
//        return;
//    }

//    // 🔎 "Bip001_Weapon"과 "Bip001 Spine"을 찾아서 할당
//    Transform[] allChildren = playerTransform.GetComponentsInChildren<Transform>();
//    foreach (Transform child in allChildren)
//    {
//        if (child.name == "Bip001_Weapon")
//            weaponTransform = child;
//        else if (child.name == "Bip001 Spine1")
//            spineTransform = child;
//    }

//    //weaponTransform = playerTransform.Find("Bip001_Weapon");
//    //spineTransform = playerTransform.Find("Bip001 Spine1");

//    if (weaponTransform == null) Debug.LogError("무기를 찾을 수 없습니다! (Bip001_Weapon)");
//    if (spineTransform == null) Debug.LogError("상체를 찾을 수 없습니다! (Bip001 Spine1)");

//    Debug.Log("ㅁㄴㅁㅇㅁ " + weaponTransform.name);
//    Debug.Log(spineTransform.name);
//}

//void LateUpdate() // 🎯 애니메이션 적용 후 회전 조정
//{


//    if (spineTransform == null || weaponTransform == null) return;

//    isAiming = Input.GetMouseButton(1); // 우클릭(조준)

//    if (isAiming)
//    {
//        Debug.Log("Spine Rotation: " + spineTransform.rotation.eulerAngles);
//        AimAtMouse();
//    }
//    else
//    {
//        // 조준 해제 시 원래 상태로 복귀
//        spineTransform.localRotation = Quaternion.Slerp(spineTransform.localRotation, Quaternion.identity, Time.deltaTime * rotationSpeed);
//    }
//}

//void AimAtMouse()
//{
//    // 🔥 마우스가 가리키는 위치를 구하기 위한 레이캐스트
//    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//    if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimLayerMask))
//    {
//        Vector3 targetPoint = hit.point; // 마우스가 가리키는 3D 위치

//        //무기 방향 회전
//        Vector3 weaponDirection = (targetPoint - weaponTransform.position).normalized;
//        Quaternion weaponRotation = Quaternion.LookRotation(weaponDirection);
//        weaponTransform.rotation = Quaternion.Slerp(weaponTransform.rotation, weaponRotation, Time.deltaTime * rotationSpeed);

//        //상체 방향 회전 (스파인)
//        Vector3 spineDirection = (targetPoint - spineTransform.position).normalized;
//        Quaternion spineRotation = Quaternion.LookRotation(spineDirection);

//        // X축과 Y축 회전 제한
//        float spineRotationX = Mathf.Clamp(spineRotation.eulerAngles.x, -maxSpineRotationX, maxSpineRotationX);
//        float spineRotationY = Mathf.Clamp(spineRotation.eulerAngles.y, -maxSpineRotationY, maxSpineRotationY);

//        //spineTransform.localRotation = Quaternion.Euler(spineRotationX, spineRotationY, 0);
//    }
//}





//using System;
//using System.Collections;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Animations.Rigging;
//using UnityEngine.Audio;

//public class PlayerController : MonoBehaviour
//{

//    public static event Action<bool> OnIsAim;

//    private CharacterController controller;
//    private Animator animator;
//    private Vector3 moveDirection;

//    public float moveSpeed = 5f;   // 이동 속도
//    public float gravity = 9.8f;   // 중력

//    protected bool isAim = false;
//    protected int atkType;          //공격타입
//    protected int dfnType;          //방어 타입

//    public float animationSpeed = 3.0f; //애니메이션 재생 속도
//    public string currentAnimation;

//    public Transform target; // sfx 소리 재생 위치
//    public MultiAimConstraint multiAimciConstraint;         //상체 뒤틀림 방지?

//    protected CharacterInfo characterInfo;


//    private bool isMoving = false;
//    private bool isPlayingFootsteps = false; // 코루틴 중복 실행 방지

//    private void OnEnable()
//    {
//        CharacterSpawnManager.OnLoadCharacterData += SetCharacterData;
//    }
//    private void OnDisable()
//    {
//        CharacterSpawnManager.OnLoadCharacterData -= SetCharacterData;
//    }

//    //spawnManager에서 가져온 캐릭터 데이터 받기
//    private void SetCharacterData(CharacterInfo info)
//    {
//        this.characterInfo = info;
//    }

//    void Start()
//    {
//        //자식 노드에서 가져오기, 캐릭터 선택을 고려하면 플레이어는 빈 오브젝트고 거기로 선택한 캐릭터를 자식으로 불러오는게 하기 25.03.06
//        controller = GetComponentInChildren<CharacterController>();
//        animator = GetComponentInChildren<Animator>();
//        //소리가 나는 위치
//        target = GameObject.Find("target").GetComponentInChildren<Transform>();
//    }

//    void Update()
//    {
//        // 입력 받기
//        float horizontal = Input.GetAxisRaw("Horizontal");
//        float vertical = Input.GetAxisRaw("Vertical");

//        // 이동 벡터 설정
//        Vector3 move = new Vector3(horizontal, 0, vertical).normalized;

//        // 현재 카메라의 회전 값 가져오기
//        Vector3 cameraForward = Camera.main.transform.forward;
//        Vector3 cameraRight = Camera.main.transform.right;
//        // Y축 방향 제거 (수직 이동 방지)
//        cameraForward.y = 0f;
//        cameraRight.y = 0f;
//        // 정규화 (길이를 1로 조정)
//        cameraForward.Normalize();
//        cameraRight.Normalize();


//        // speed 값 즉각 반영 (키를 누르는 즉시 애니메이션 전환됨)
//        float speed = move.magnitude;

//        isMoving = move.magnitude > 0;
//        animator.SetBool("isMoving", isMoving);

//        //animator.speed = animationSpeed;

//        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);  // 1보다 크면 애니메이션이끝인 상태?
//        if (stateInfo.IsName(currentAnimation) && stateInfo.normalizedTime >= 1.0f)
//        {
//            currentAnimation = "isShoot";
//            animator.Play(currentAnimation);
//        }


//        //조준시 이동 제어를 위해,우선 조준에 대한 코드는 GunController대신 여기서 처리
//        if (Input.GetMouseButton(1))
//        {
//            //multiAimciConstraint.data.offset = new Vector3(-30, 0, 0);
//            isAim = true;
//            OnIsAim?.Invoke(isAim);
//        }
//        else
//        {
//            //multiAimciConstraint.data.offset = new Vector3(0, 0, 0);
//            isAim = false;
//            OnIsAim?.Invoke(isAim);
//        }

//        //Vector3 moveXZ = (cameraForward * vertical + cameraRight * horizontal) * moveSpeed;

//        //// **이전 Y값 유지!**
//        //moveDirection.x = moveXZ.x;
//        //moveDirection.z = moveXZ.z;

//        // 중력 적용
//        if (!controller.isGrounded)
//        {
//            moveDirection.y -= gravity * Time.deltaTime;
//            Debug.Log(moveDirection.y);
//        }
//        else
//        {
//            moveDirection.y = 0f;
//        }

//        //moveDirection = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);

//        // 입력 값을 카메라 기준으로 변환
//        moveDirection = (cameraForward * vertical + cameraRight * horizontal);

//        if (moveDirection.magnitude > 1f)
//        {
//            moveDirection.Normalize();
//        }

//        // 이동 처리
//        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

//        if (isMoving && controller.isGrounded && !isPlayingFootsteps)
//        {
//            StartCoroutine(PlayFootsteps());
//        }

//        if (isAim)
//        {
//            //조준시에는 이동할 때 보다는 빨리 회전 2025-03-10 23:16
//            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
//            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 7f);
//        }
//        else if (moveDirection != Vector3.zero)// 이동 중이면 이동 방향으로 캐릭터 회전
//        {
//            //소리가 겹침 - Update 밖에서 걸어줘야됨
//            //SoundManager.Instance.PlayWalkSfx("walkNormal1");
//            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
//            float rotationSpeed = 300f;
//            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
//        }
//    }

//    private IEnumerator PlayFootsteps()
//    {
//        isPlayingFootsteps = true;
//        while (controller.isGrounded)
//        {
//            //SoundManager.Instance.PlayWalkSfx("walkNormal1", controller.transform.position);
//            yield return new WaitForSeconds(0.5f);
//        }
//        isPlayingFootsteps = false;
//    }


//    public void OnTriggerEnter(Collider other)
//    {
//        //if (other.gameObject.CompareTag("Enemy"))
//        //{
//        //    //animator.SetTrigger("isDeath");
//        //    currentHP -= 250;

//        //    GetComponentInChildren<CharacterController>().enabled = false;
//        //    other.gameObject.transform.position = Vector3.zero;
//        //    GetComponentInChildren<CharacterController>().enabled = true;
//        //    Debug.Log(other.gameObject.transform.position);
//        //    Debug.Log(currentHP);

//        //}
//    }

//    //발소리 제어 임시 코드 - 레이캐스트
//    //public void FootStepSoundOn()
//    //{
//    //    if (Physics.Raycast(transform.position, transform.forward, out hit, 10.0f, itemLayer))
//    //    {
//    //        if (hit.ColliderHit.tag == "Snow")
//    //        {
//    //            audioSource.PlayOneShot(audioClipFire); //발소리재생
//    //        }
//    //        else if (hit.ColliderHit.tag == "Sand")
//    //        {
//    //            audioSource.PlayOneShot(audioClipFire); //발소리재생
//    //        }
//    //    }
//    //}


//}
