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

    public float moveSpeed = 5f;   // �̵� �ӵ�
    public float gravity = 9.8f;   // �߷�

    protected bool isAim = false;
    protected int atkType;          //����Ÿ��
    protected int dfnType;          //��� Ÿ��

    public float animationSpeed = 3.0f; //�ִϸ��̼� ��� �ӵ�
    public string currentAnimation;

    public Transform target; // sfx �Ҹ� ��� ��ġ
    public MultiAimConstraint multiAimciConstraint;         //��ü ��Ʋ�� ����?

    protected CharacterInfo characterInfo;


    private bool isMoving = false;
    private bool isPlayingFootsteps = false; // �ڷ�ƾ �ߺ� ���� ����

    private void OnEnable()
    {
        CharacterSpawnManager.OnLoadCharacterData += SetCharacterData;
    }
    private void OnDisable()
    {
        CharacterSpawnManager.OnLoadCharacterData -= SetCharacterData;
    }

    //spawnManager���� ������ ĳ���� ������ �ޱ�
    private void SetCharacterData(CharacterInfo info)
    {
        this.characterInfo = info;
    }

    void Start()
    {
        //�ڽ� ��忡�� ��������, ĳ���� ������ ����ϸ� �÷��̾�� �� ������Ʈ�� �ű�� ������ ĳ���͸� �ڽ����� �ҷ����°� �ϱ� 25.03.06
        controller = GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        //�Ҹ��� ���� ��ġ
        target = GameObject.Find("target").GetComponentInChildren<Transform>();
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

        isMoving = move.magnitude > 0;
        animator.SetBool("isMoving", isMoving);

        //���ؽ� �̵� ��� ����,�켱 ���ؿ� ���� �ڵ�� GunController��� ���⼭ ó��
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

        //// **���� Y�� ����!**
        //moveDirection.x = moveXZ.x;
        //moveDirection.z = moveXZ.z;

        // �߷� ����
        if (!controller.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
            Debug.Log(moveDirection.y); 
        }
        else
        {
            moveDirection.y = 0f;
        }

         //moveDirection = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);

        // �Է� ���� ī�޶� �������� ��ȯ
        moveDirection = (cameraForward * vertical + cameraRight * horizontal);

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        // �̵� ó��
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);


        //controller.Move(moveDirection * Time.deltaTime);

        // **Player ������Ʈ�� ȸ���� ���� ���� (X=90 ����)**
        //transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        //// **ĳ������ ���� �𵨸� ȸ�� (Y�ุ ȸ��)**
        //if (move.magnitude > 0)
        //{
        //    Transform characterTransform = controller.transform; // CharacterController�� �ִ� ������Ʈ
        //    Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
        //    characterTransform.rotation = Quaternion.Slerp(characterTransform.rotation, newRotation, Time.deltaTime * 10f);
        //}

        //if (isMoving && controller.isGrounded && !isPlayingFootsteps)
        //{
        //    StartCoroutine(PlayFootsteps());
        //}

        if (isAim)
        {
            //���ؽÿ��� �̵��� �� ���ٴ� ���� ȸ�� 2025-03-10 23:16
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 7f);
        }
        else if (moveDirection != Vector3.zero)// �̵� ���̸� �̵� �������� ĳ���� ȸ��
        {
            //�Ҹ��� ��ħ - Update �ۿ��� �ɾ���ߵ�
            //SoundManager.Instance.PlayWalkSfx("walkNormal1");
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            float rotationSpeed = 300f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator PlayFootsteps()
    {
        isPlayingFootsteps = true;
        while (controller.isGrounded)
        {
            //SoundManager.Instance.PlayWalkSfx("walkNormal1", controller.transform.position);
            yield return new WaitForSeconds(0.5f);
        }
        isPlayingFootsteps = false;
    }

    //�߼Ҹ� ���� �ӽ� �ڵ� - ����ĳ��Ʈ
    public void FootStepSoundOn()
    {
        //if (Physics.Raycast(transform.position, transform.forward, out hit, 10.0f, itemLayer))
        //{
        //    if (hit.ColliderHit.tag == "Snow")
        //    {
        //        audioSource.PlayOneShot(audioClipFire); //�߼Ҹ����
        //    }
        //    else if (hit.ColliderHit.tag == "Sand")
        //    {
        //        audioSource.PlayOneShot(audioClipFire); //�߼Ҹ����
        //    }
        //}
        //SoundManager.Instance.PlayWalkSfx("walkNormal1", controller.transform.position);
    }



}
