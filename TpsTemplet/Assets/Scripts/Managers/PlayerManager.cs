using System;
using UnityEngine;

//�÷��̾��� ü��, ���, ��Ȱ, �ǰ� �� ���¿� ���� �Ŵ����� �ٷ� �ڵ�
//Json ������ �����͸� �ҷ����� ������ ���� ����
//palyer �� ������Ʈ�� �ƴ� ĳ���� ��ü�� �޾��ٰ� -> �ݶ��̴� ������ ����
public class PlayerManager : MonoBehaviour
{
    public static event Action<int, int> UpdateHPUI;  //gamePlayUi���� ź���� ǥ�� �ϱ� ����

    public int playerHP = 2500;     //�÷��̾� ü��
    private int currentHP;           //���� �÷��̾��� ü��

    void Start()
    {
        currentHP = playerHP;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateHPUI?.Invoke(currentHP, playerHP);

    }
}
