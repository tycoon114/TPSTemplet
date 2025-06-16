using System;
using Unity.Netcode;
using UnityEngine;

//플레이어의 체력, 사망, 부활, 피격 등 상태에 관한 매니저를 다룰 코드
//발소리에 관한 것도 이곳에서 처리함
public class PlayerManager :  NetworkBehaviour
{
    public static event Action<float, float> UpdateHPUI;  //gamePlayUi에서 탄약을 표시 하기 위함

    private float playerHP = 2500;     //플레이어 체력
    private float currentHP;           //현재 플레이어의 체력
    public LayerMask groundLayer;        //바닥 검사용
    private Animator animator;

    private bool isInvincibility;       //무적 상태인지?

    void Start()
    {
        currentHP = playerHP;
        groundLayer = LayerMask.GetMask("Ground", "Enemy", "Player", "EnemyPlayer");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner)
            UpdateHPUI?.Invoke(currentHP, playerHP);
    }

    //이 데미지를 계산 하는것은 네트워크가 처리해줘야됨
    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + "  데미지 받음");

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