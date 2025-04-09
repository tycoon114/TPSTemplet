using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

//플레이어의 체력, 사망, 부활, 피격 등 상태에 관한 매니저를 다룰 코드
//발소리에 관한 것도 이곳에서 처리함
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
    public LayerMask groundLayer;        //바닥 검사용


    private bool isInvincibility;       //무적 상태인지?

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
        groundLayer = LayerMask.GetMask("Ground", "Enemy", "Player", "EnemyPlayer");
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

    public void FootStepSoundOn()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        Vector3 rayDirection = Vector3.down;

        //이때 바닥으로 쓸 콜라이더는 Ground보다 밑에 설치해야됨
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, 10.0f, groundLayer))
        {
            if (hit.collider.CompareTag("Sand"))
            {
                SoundManager.Instance.PlayWalkSfx("runSand1");  //모래 발소리재생
                return;
            }
            else if (hit.collider.CompareTag("Snow"))
            {
                SoundManager.Instance.PlayWalkSfx("runSnow");   //눈 발소리재생
                return;
            }
            else if (hit.collider.CompareTag("DamageZone"))
            {
                SoundManager.Instance.PlayWalkSfx("runSnow");   //데미지 받는 UI 확인용 
                return;
            }
        }
        SoundManager.Instance.PlayWalkSfx("walk1");
    }

    //게임 시작 직후, 부활 후, 스폰장소에서 무적
    // IEnumerator Invincibility() {

        
    //}


}
