using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Audio;

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
    public float animationSpeed = 3.0f; //�ִϸ��̼� ��� �ӵ�
    public string currentAnimation;

    public Transform target; // ��ü�� ���� ��


    public MultiAimConstraint multiAimciConstraint;         //��ü ��Ʋ�� ����?
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

        //animator.speed = animationSpeed;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);  // 1���� ũ�� �ִϸ��̼��̳��� ����?
        if (stateInfo.IsName(currentAnimation) && stateInfo.normalizedTime >= 1.0f)
        {
            currentAnimation = "isShoot";
            animator.Play(currentAnimation);
        }


        //���ؽ� �̵� ��� ����,�켱 ���ؿ� ���� �ڵ�� GunController��� ���⼭ ó��
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
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (isAim)
        {
            //AimController���� ó�� - �ȵǸ� �ּ� Ǯ�� 2025- 02-26
            //transform.rotation = Quaternion.LookRotation(cameraForward); // ������ ���� ����

            //���ؽÿ��� �̵��� �� ���ٴ� ���� ȸ�� 2025-03-10 23:16
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 7f);
            //UpdateAimTarget();

        }
        else if (moveDirection != Vector3.zero)// �̵� ���̸� �̵� �������� ĳ���� ȸ��
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

    //�߼Ҹ� ���� �ӽ� �ڵ� - ����ĳ��Ʈ
    //public void FootStepSoundOn()
    //{
    //    if (Physics.Raycast(transform.position, transform.forward, out hit, 10.0f, itemLayer))
    //    {
    //        if (hit.ColliderHit.tag == "Wood")
    //        {
    //            audioSource.PlayOneShot(audioClipFire); //�߼Ҹ����
    //        }
    //        else if (hit.ColliderHit.tag == "Wood")
    //        {
    //            audioSource.PlayOneShot(audioClipFire); //�߼Ҹ����
    //        }
    //    }
    //}


}
