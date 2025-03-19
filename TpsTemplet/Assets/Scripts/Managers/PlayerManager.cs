using System;
using UnityEngine;

//플레이어의 체력, 사망, 부활, 피격 등 상태에 관한 매니저를 다룰 코드
//Json 형태의 데이터를 불러오는 식으로 만들 예정
//palyer 빈 오브젝트가 아닌 캐릭터 자체에 달아줄것 -> 콜라이더 접근을 위함
public class PlayerManager : MonoBehaviour
{
    public static event Action<float, float> UpdateHPUI;  //gamePlayUi에서 탄약을 표시 하기 위함

    public float playerHP = 2500;     //플레이어 체력
    private float currentHP;           //현재 플레이어의 체력

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
        //우선 임시로 태그로 달아두고, 이후에 총에 맞는거로 수정
        //if (other.CompareTag("getHit")) {
        //    //animator.setTrigger("Damage");
        //    currentHP -= 30; // 임시로 30데미지

        //}
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + "  데미지 받음");

        currentHP -= damage;
        if (currentHP <= 0)
        {
            //죽는 애니메이션 재생
        }
    }


}
