using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//플레이어의 체력, 사망, 부활, 피격 등 상태에 관한 매니저를 다룰 코드
//Json 형태의 데이터를 불러오는 식으로 만들 예정
//palyer 빈 오브젝트가 아닌 캐릭터 자체에 달아줄것 -> 콜라이더 접근을 위함
public class PlayerManager : MonoBehaviour
{
    //우선은 싱글톤을 썼지만 멀티플레이에세는 싱글톤을 쓰면 안됨
    //이후에 게임 모드에 따라 발동하도록 설정하기
    public static PlayerManager Instance
    {
        get;
        private set;
    }

    public static event Action<float, float> UpdateHPUI;  //gamePlayUi에서 탄약을 표시 하기 위함

    public float playerHP = 2500;     //플레이어 체력
    private float currentHP;           //현재 플레이어의 체력

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
        //우선 임시로 태그로 달아두고, 이후에 총에 맞는거로 수정
        //if (other.CompareTag("getHit")) {
        //    //animator.setTrigger("Damage");
        //    currentHP -= 30; // 임시로 30데미지
        //}

        //문도 여기서 트리거 작동
        //이때 DoorController를 참조한다.

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
