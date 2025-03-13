using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Audio;

public class PlayerController3 : MonoBehaviour
{
    public float moveSpeed = 5f;   // 이동 속도
    public float gravity = 9.8f;   // 중력
    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;
    protected bool isAim = false;
    protected int atkType;          //공격타입
    protected int dfnType;          //방어 타입



    public AudioClip walk;
    public float animationSpeed = 3.0f; //애니메이션 재생 속도
    public string currentAnimation;

    public Transform target; // 상체가 꺽일 곳


    public MultiAimConstraint multiAimciConstraint;         //상체 뒤틀림 방지?
    void Start()
    {
        //자식 노드에서 가져오기, 캐릭터 선택을 고려하면 플레이어는 빈 오브젝트고 거기로 선택한 캐릭터를 자식으로 불러오는게 하기 25.03.06
        controller = GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>();

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

        bool isMoving = move.magnitude > 0;
        animator.SetBool("isMoving", isMoving);

        //animator.speed = animationSpeed;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);  // 1보다 크면 애니메이션이끝인 상태?
        if (stateInfo.IsName(currentAnimation) && stateInfo.normalizedTime >= 1.0f)
        {
            currentAnimation = "isShoot";
            animator.Play(currentAnimation);
        }


        //조준시 이동 제어를 위해,우선 조준에 대한 코드는 GunController대신 여기서 처리
        if (Input.GetMouseButton(1))
        {
            //multiAimciConstraint.data.offset = new Vector3(-30, 0, 0);
            animator.SetLayerWeight(1, 1);
            isAim = true;


        }
        else
        {
            //multiAimciConstraint.data.offset = new Vector3(0, 0, 0);
            animator.SetLayerWeight(1, 0);
            isAim = false;
        }

        // 중력 적용
        if (!controller.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y = 0; // 바닥에 닿으면 중력 초기화
        }

        // 입력 값을 카메라 기준으로 변환
        moveDirection = (cameraForward * vertical + cameraRight * horizontal);
        // 이동 처리
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (isAim)
        {
            //AimController에서 처리 - 안되면 주석 풀것 2025- 02-26
            //transform.rotation = Quaternion.LookRotation(cameraForward); // 정조준 방향 유지

            //조준시에는 이동할 때 보다는 빨리 회전 2025-03-10 23:16
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 7f);
            //UpdateAimTarget();

        }
        else if (moveDirection != Vector3.zero)// 이동 중이면 이동 방향으로 캐릭터 회전
        {

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            float rotationSpeed = 300f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("Enemy"))
        //{
        //    //animator.SetTrigger("isDeath");
        //    currentHP -= 250;

        //    GetComponentInChildren<CharacterController>().enabled = false;
        //    other.gameObject.transform.position = Vector3.zero;
        //    GetComponentInChildren<CharacterController>().enabled = true;
        //    Debug.Log(other.gameObject.transform.position);
        //    Debug.Log(currentHP);

        //}
    }


    //void UpdateAimTarget()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    target.position = ray.GetPoint(10.0f);
    //}

    //public void FootStepSoundOn()
    //{


    //}

    //발소리 제어 임시 코드 - 레이캐스트
    //public void FootStepSoundOn()
    //{
    //    if (Physics.Raycast(transform.position, transform.forward, out hit, 10.0f, itemLayer))
    //    {
    //        if (hit.ColliderHit.tag == "Wood")
    //        {
    //            audioSource.PlayOneShot(audioClipFire); //발소리재생
    //        }
    //        else if (hit.ColliderHit.tag == "Wood")
    //        {
    //            audioSource.PlayOneShot(audioClipFire); //발소리재생
    //        }
    //    }
    //}


}
