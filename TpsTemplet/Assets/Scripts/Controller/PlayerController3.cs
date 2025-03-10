using UnityEngine;

public class PlayerController3 : MonoBehaviour
{
    public float moveSpeed = 5f;   // �̵� �ӵ�
    public float gravity = 9.8f;   // �߷�
    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;
    protected bool isAim = false;
    protected int atkType;          //����Ÿ��
    protected int dfnType;          //��� Ÿ��

    public AudioClip walk;

    void Start()
    {
        //�ڽ� ��忡�� ��������, ĳ���� ������ ����ϸ� �÷��̾�� �� ������Ʈ�� �ű�� ������ ĳ���͸� �ڽ����� �ҷ����°� �ϱ� 25.03.06
        controller = GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // �Է� �ޱ�
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // �̵� ���� ����
        Vector3 move = new Vector3(horizontal, 0, vertical).normalized;

        // ���� ī�޶��� ȸ�� �� ��������
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        // Y�� ���� ���� (���� �̵� ����)
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        // ����ȭ (���̸� 1�� ����)
        cameraForward.Normalize();
        cameraRight.Normalize();


        // speed �� �ﰢ �ݿ� (Ű�� ������ ��� �ִϸ��̼� ��ȯ��)
        float speed = move.magnitude;

        bool isMoving = move.magnitude > 0;
        animator.SetBool("isMoving", isMoving);

        //@TK(25.02.24)
        //animator.SetFloat("FactorX", moveX);
        //animator.SetFloat("FactorZ", moveZ);

        //���ؽ� �̵� ��� ����,�켱 ���ؿ� ���� �ڵ�� GunController��� ���⼭ ó��
        if (Input.GetMouseButton(1))
        {
            isAim = true;
        }
        else
        {
            isAim = false;
        }

        // �߷� ����
        if (!controller.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y = 0; // �ٴڿ� ������ �߷� �ʱ�ȭ
        }

        // �Է� ���� ī�޶� �������� ��ȯ
        moveDirection = (cameraForward * vertical + cameraRight * horizontal);
        // �̵� ó��
        //transform.position += moveDirection * moveSpeed * Time.deltaTime;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (isAim)
        {
            //AimController���� ó�� - �ȵǸ� �ּ� Ǯ�� 2025- 02-26
            transform.rotation = Quaternion.LookRotation(cameraForward); // ������ ���� ����
        }
        else if (moveDirection != Vector3.zero)// �̵� ���̸� �̵� �������� ĳ���� ȸ��
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }
}
