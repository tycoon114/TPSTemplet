using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//�÷��̾��� ü��, ���, ��Ȱ, �ǰ� �� ���¿� ���� �Ŵ����� �ٷ� �ڵ�
//Json ������ �����͸� �ҷ����� ������ ���� ����
//palyer �� ������Ʈ�� �ƴ� ĳ���� ��ü�� �޾��ٰ� -> �ݶ��̴� ������ ����
public class PlayerManager : MonoBehaviour
{
    //�켱�� �̱����� ������ ��Ƽ�÷��̿����� �̱����� ���� �ȵ�
    //���Ŀ� ���� ��忡 ���� �ߵ��ϵ��� �����ϱ�
    public static PlayerManager Instance
    {
        get;
        private set;
    }

    public static event Action<float, float> UpdateHPUI;  //gamePlayUi���� ź���� ǥ�� �ϱ� ����

    public float playerHP = 2500;     //�÷��̾� ü��
    private float currentHP;           //���� �÷��̾��� ü��

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        currentHP = playerHP;

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
            //�״� �ִϸ��̼� ���
        }
    }

}
