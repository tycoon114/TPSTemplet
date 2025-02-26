using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 5f;   // 이동 속도
    public float gravity = 9.8f;   // 중력
    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;
    bool isAim = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        // 입력 받기
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // 이동 벡터 설정
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

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


        // 이동 중이면 방향을 변경
        //여기서 뒤로 가는 경우 방향을 변경하지 않도록 해줘야됨 ,&& Input.GetKeyDown(KeyCode.W) 
        //if (isMoving )
        //{
        //    transform.forward = move;
        //}

        //누른 방향으로 회전2

        //if (!(moveX == 0 && moveZ == 0)) {
        //    //이동과 회전을 같이 처리
        //    transform.position += move * speed * Time.deltaTime;
        //    //회전하는 부분
        //    transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(move),Time.deltaTime);
        //}

        if (Input.GetMouseButton(1))
        {
            isAim = true;
            animator.SetBool("isAim", isAim);
        }
        else
        {
            isAim = false;
            animator.SetBool("isAim", isAim);
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

        // 방향과 이동 처리 (구)
        //moveDirection = move * moveSpeed;
        //controller.Move(moveDirection * Time.deltaTime);


        // 입력 값을 카메라 기준으로 변환
        moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;
        // 이동 처리
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (isAim)
        {
            transform.rotation = Quaternion.LookRotation(cameraForward); // 정조준 방향 유지
        }
        else if (moveDirection != Vector3.zero)// 이동 중이면 이동 방향으로 캐릭터 회전
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 10f);
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        //// 카메라도 이동 방향을 따라 조금씩 회전
        //Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        //Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetRotation, Time.deltaTime * 2f);


    }

}
