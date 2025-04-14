using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

//�÷��̾��� ü��, ���, ��Ȱ, �ǰ� �� ���¿� ���� �Ŵ����� �ٷ� �ڵ�
//�߼Ҹ��� ���� �͵� �̰����� ó����
//Json ������ �����͸� �ҷ����� ������ ���� ����
//palyer �� ������Ʈ�� �ƴ� ĳ���� ��ü�� �޾��ٰ� -> �ݶ��̴� ������ ����
public class PlayerManager :  NetworkBehaviour
{
    public static event Action<float, float> UpdateHPUI;  //gamePlayUi���� ź���� ǥ�� �ϱ� ����

    public float playerHP = 2500;     //�÷��̾� ü��
    private float currentHP;           //���� �÷��̾��� ü��
    public LayerMask groundLayer;        //�ٴ� �˻��
    private Animator animator;

    private bool isInvincibility;       //���� ��������?

    void Start()
    {
        currentHP = playerHP;
        groundLayer = LayerMask.GetMask("Ground", "Enemy", "Player", "EnemyPlayer");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHPUI?.Invoke(currentHP, playerHP);
    }

    private void OnTriggerEnter(Collider other)
    {
        //�켱 �ӽ÷� �±׷� �޾Ƶΰ�, ���Ŀ� �ѿ� �´°ŷ� ����
        //if (other.CompareTag("getHit")) {
        //    //animator.setTrigger("Damage");
        //    currentHP -= 30; // �ӽ÷� 30������
        //}

        //���� ���⼭ Ʈ���� �۵�
        //�̶� DoorController�� �����Ѵ�.
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + "  ������ ����");

        currentHP -= damage;
        if (currentHP <= 0)
        {
            animator.SetTrigger("isDeath");
        }
        Debug.Log(currentHP);
    }

    public void FootStepSoundOn()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        Vector3 rayDirection = Vector3.down;

        //�̶� �ٴ����� �� �ݶ��̴��� Ground���� �ؿ� ��ġ�ؾߵ�
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, 10.0f, groundLayer))
        {
            if (hit.collider.CompareTag("Sand"))
            {
                SoundManager.Instance.PlayWalkSfx("runSand1");  //�� �߼Ҹ����
                return;
            }
            else if (hit.collider.CompareTag("Snow"))
            {
                SoundManager.Instance.PlayWalkSfx("runSnow");   //�� �߼Ҹ����
                return;
            }
            else if (hit.collider.CompareTag("DamageZone"))
            {
                SoundManager.Instance.PlayWalkSfx("runSnow");   //������ �޴� UI Ȯ�ο� 
                return;
            }
        }
        SoundManager.Instance.PlayWalkSfx("walk1");
    }

    //���� ���� ����, ��Ȱ ��, ������ҿ��� ����
    // IEnumerator Invincibility() {

        
    //}

}
