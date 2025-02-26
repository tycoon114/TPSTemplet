using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum PlayerState
    {
        Idle,
        Move,
        Attack,
        Dead
    }

    public PlayerState _state;


    public float speed = 5f;       // 이동 속도
    public float gravity = -9.81f; // 중력 값
    public float jumpSpeed = 3.0f;

    private CharacterController controller;
    private Vector3 velocity;
    private Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        _state = PlayerState.Idle;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //float horizontal = Input.GetAxis("Horizontal"); // A, D 또는 좌우 화살표
        //float vertical = Input.GetAxis("Vertical");   // W, S 또는 상하 화살표

        //Vector3 move = transform.right * horizontal + transform.forward * vertical;
        //controller.Move(move * speed * Time.deltaTime); // 이동 처리




        //if (move != Vector3.zero)
        //{
        //    // 진행 방향으로 캐릭터 회전
        //    transform.rotation = Quaternion.Euler(0, Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg, 0);
        //    anim.SetBool("IsMove", true);
        //}
        //else
        //{
        //    anim.SetBool("IsMoveEnd", true);
        //    anim.SetBool("IsMove", false);
        //}


        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        ////점프 하는 중에 점프는 불가능, 하지만 이동은 가능
        //if (controller.isGrounded)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //        velocity.y = 7.5f;
        //}


    }
}
