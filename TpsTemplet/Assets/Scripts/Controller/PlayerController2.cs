using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 5f;   // 이동 속도
    public float gravity = 9.8f;   // 중력
    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;
    protected bool isAim = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    //fixed는 물리 처리 할떄 사용

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

        //@TK(25.02.24)
        //animator.SetFloat("FactorX", moveX);
        //animator.SetFloat("FactorZ", moveZ);
        if (Input.GetMouseButton(1))
        {
            isAim = true;
        }
        else
        {
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
        //transform.position += moveDirection * moveSpeed * Time.deltaTime;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (isAim)
        {
            //AimController에서 처리 - 안되면 주석 풀것 2025- 02-26
            transform.rotation = Quaternion.LookRotation(cameraForward); // 정조준 방향 유지
        }
        else if (moveDirection != Vector3.zero)// 이동 중이면 이동 방향으로 캐릭터 회전
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 10f);
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }
}
