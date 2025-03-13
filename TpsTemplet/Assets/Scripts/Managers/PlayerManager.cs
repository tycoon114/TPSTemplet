using System;
using UnityEngine;

//플레이어의 체력, 사망, 부활, 피격 등 상태에 관한 매니저를 다룰 코드
//Json 형태의 데이터를 불러오는 식으로 만들 예정
//palyer 빈 오브젝트가 아닌 캐릭터 자체에 달아줄것 -> 콜라이더 접근을 위함
public class PlayerManager : MonoBehaviour
{
    public static event Action<int, int> UpdateHPUI;  //gamePlayUi에서 탄약을 표시 하기 위함

    public int playerHP = 2500;     //플레이어 체력
    private int currentHP;           //현재 플레이어의 체력

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
