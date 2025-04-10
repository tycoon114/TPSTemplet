using System.Collections;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


//미니 게임용 적 관련 코드
//현재는 트리거 관련 테스트용 코드로 사용[죽으면 부활 지역으로 이동하는 이벤트] 2025- 03 - 12
//이것으로 데미지를 주고, 받는 테스트 용으로. 이후에는 적Ai로 사용 2025 03 13

//상태는 애니메이션 기준
public enum enemyState
{
    idle,
    attack,
    panic,
    death,
    evade,       //회피 - 애니메이션은 move 사용
    patroll,     //순찰 - move 사용
    chase       // 추적 - move 사용
}

public class EnemyManger : MonoBehaviour
{
    //    public enemyState currentState = enemyState.idle;
    //    public float attackRange = 1.0f;    //공격 범위. 이후 적 종류에 따라 변경
    //    public float attackDelay = 2.0f;    //공격 딜레이
    //    public float moveSpeed = 2.0f;      //이동 속도
    //    private float nextAttackTime = 0.0f;    //다음 공격 시간 관리
    //    public Transform[] patrolPoints;        //순찰 경로 지점들
    //    private int currentPoint = 0;       //현재 순찰 경로 지점 인덱스
    //    private float trackingRange = 4.0f;      // 추적 범위
    //    private bool isAttack = false;          //공격 상태
    //    private float evadeRange = 5.0f;      //회피 범위

    //    private float enemyHP = 100.0f;    //현재는 스위퍼의 체력
    //    private float distanceToTarget;     //타겟과의 거리 계산 값
    //    private bool isWaiting = false;     //상태 전환 후 대기 상태 여부
    //    public float idleTime = 2.0f;       //각 상태 전환 후 대기 시간

    //    private Animator animator;

    //    private Coroutine stateRoutine;         //현재 실행중인 코루틴

    //    private NavMeshAgent navAgent;

    //    private bool isJumping = false;     //onGround 로 바뀔수 있음
    //    private Rigidbody rb;
    //    public float jumpHeight = 2.0f;
    //    public float jumpDuration = 1.0f;
    //    private NavMeshLink[] navMeshLinks;

    //    void Start()
    //    {
    //        animator = GetComponent<Animator>();
    //        navAgent = GetComponent<NavMeshAgent>();
    //        ChangeState(currentState);
    //        rb = GetComponent<Rigidbody>();
    //        if (rb == null)
    //        {
    //            rb = gameObject.AddComponent<Rigidbody>();
    //        }
    //        rb.isKinematic = true;

    //        navMeshLinks = FindObjectsOfType<NavMeshLink>();
    //    }
    //    void Update()
    //    {
    //        if (PlayerManager.Instance != null)
    //        {
    //            distanceToTarget = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
    //        }
    //    }

    //    public void ChangeState(enemyState newState)
    //    {
    //        //점프 중일 경우 리턴
    //        if (isJumping) return;

    //        if (stateRoutine != null)
    //        {
    //            Debug.Log("현재 상태   " + stateRoutine + "   " + newState);
    //            StopCoroutine(stateRoutine);
    //        }
    //        currentState = newState;    //바꿔줄 상태

    //        //if문으로 할지 스위치할지 고민중
    //        //if (currentState == enemyState.idle)
    //        //{
    //        //    stateRoutine = StartCoroutine(Idle());
    //        //}

    //        switch (currentState)
    //        {
    //            case enemyState.idle:
    //                stateRoutine = StartCoroutine(Idle());
    //                break;
    //            case enemyState.patroll:
    //                stateRoutine = StartCoroutine(Patroll());
    //                break;
    //            case enemyState.attack:
    //                stateRoutine = StartCoroutine(Attack());
    //                break;
    //            case enemyState.evade:
    //                stateRoutine = StartCoroutine(Evade());
    //                break;
    //            case enemyState.panic:
    //                stateRoutine = StartCoroutine(Panic());
    //                break;
    //            case enemyState.death:
    //                stateRoutine = StartCoroutine(Death());
    //                break;
    //            case enemyState.chase:
    //                stateRoutine = StartCoroutine(Chase());
    //                break;
    //        }
    //    }

    //    private IEnumerator Idle()
    //    {
    //        Debug.Log(gameObject.name + "대기");
    //        animator.Play("Idle");

    //        while (currentState == enemyState.idle)
    //        {
    //            float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);

    //            if (distance < trackingRange)
    //            {
    //                ChangeState(enemyState.chase);
    //            }
    //            else if (distance < attackRange)
    //            {
    //                ChangeState(enemyState.attack);
    //            }

    //            yield return null;
    //        }
    //    }

    //    private IEnumerator Chase()
    //    {
    //        Debug.Log(gameObject.name + "추적");

    //        while (currentState == enemyState.chase)
    //        {

    //            float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);

    //            Vector3 direction = (PlayerManager.Instance.transform.position - transform.position).normalized;
    //            navAgent.speed = moveSpeed;
    //            navAgent.isStopped = false;
    //            navAgent.destination = PlayerManager.Instance.transform.position;

    //            //transform.position += direction * moveSpeed * Time.deltaTime;
    //            //transform.LookAt(target.position);
    //            animator.SetBool("isMove", true);

    //            if (distance < attackRange)
    //            {
    //                ChangeState(enemyState.attack);
    //                yield break;
    //            }

    //            if (distance > trackingRange * 1.5f)
    //            {
    //                ChangeState(enemyState.patroll);
    //                yield break;
    //            }
    //            yield return null;
    //        }
    //    }

    //    private IEnumerator Patroll()
    //    {
    //        Debug.Log(gameObject.name + "순찰");

    //        while (currentState == enemyState.patroll)
    //        {
    //            if (patrolPoints.Length > 0)
    //            {
    //                animator.SetBool("isMove", true);

    //                Transform targetPoint = patrolPoints[currentPoint];
    //                Vector3 direction = (targetPoint.position - transform.position).normalized;

    //                navAgent.speed = moveSpeed;
    //                navAgent.isStopped = false;
    //                navAgent.destination = PlayerManager.Instance.transform.position;

    //                //transform.position += direction * moveSpeed * Time.deltaTime;
    //                //transform.LookAt(targetPoint.transform);

    //                if (navAgent.isOnOffMeshLink)
    //                {
    //                    StartCoroutine(JumpAcrossLink());
    //                }


    //                if (Vector3.Distance(transform.position, targetPoint.position) < 0.3f)
    //                {
    //                    currentPoint = (currentPoint + 1) % patrolPoints.Length;
    //                }

    //                float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
    //                if (distance < trackingRange)
    //                {
    //                    ChangeState(enemyState.chase);
    //                }
    //                else if (distance < attackRange)
    //                {
    //                    ChangeState(enemyState.attack);
    //                }
    //                yield return null;
    //            }
    //        }
    //    }

    //    private IEnumerator Attack()
    //    {
    //        Debug.Log(gameObject.name + "공격");


    //        navAgent.isStopped = true;
    //        navAgent.destination = PlayerManager.Instance.transform.position;


    //        animator.SetTrigger("attack");
    //        //transform.LookAt(target.position);
    //        //지연 시간은 몹에 따라 추가 제거
    //        yield return new WaitForSeconds(attackDelay);

    //        float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
    //        if (distance > attackRange)
    //        {
    //            //공격 범위를 벗어 났을 경우
    //            ChangeState(enemyState.chase);
    //        }
    //        else
    //        {
    //            ChangeState(enemyState.attack);
    //        }


    //    }
    //    private IEnumerator Evade()
    //    {
    //        Debug.Log(gameObject.name + "도망");
    //        animator.SetBool("isMove", true);

    //        Vector3 evadeDirection = (transform.position - PlayerManager.Instance.transform.position).normalized;
    //        float evadeTime = 3.0f;
    //        float timer = 0.0f;

    //        //바라볼 대상이 없기 때문에 LookAt 사용 안함

    //        //밑 두줄은 while 문안에 넣어서 돌려도 되긴함 - 상황에 따라 맞게 변경
    //        //Quaternion targetRotation = Quaternion.LookRotation(evadeDirection);
    //        //transform.rotation = targetRotation;

    //        while (currentState == enemyState.evade && timer < evadeTime)
    //        {
    //            //transform.position += evadeDirection * moveSpeed * Time.deltaTime;
    //            timer += Time.deltaTime;
    //            yield return null;
    //        }
    //        ChangeState(enemyState.idle);
    //    }

    //    private IEnumerator Panic()
    //    {
    //        Debug.Log(gameObject.name + "  혼란");

    //        yield return null;
    //    }
    //    private IEnumerator Death()
    //    {
    //        Debug.Log(gameObject.name + "  사망");
    //        animator.SetTrigger("isDeath");

    //        yield return new WaitForSeconds(2.0f);
    //        gameObject.SetActive(false);
    //    }




    //    public void TakeDamage(float damage)
    //    {
    //        Debug.Log(gameObject.name + "  데미지 받음");

    //        enemyHP -= damage;
    //        if (enemyHP <= 0)
    //        {
    //            ChangeState(enemyState.death);
    //        }
    //    }

    //    private IEnumerator JumpAcrossLink()
    //    {
    //        Debug.Log(gameObject.name + "  jumpppp");
    //        isJumping = true;
    //        //에이전트 중단
    //        navAgent.isStopped = true;

    //        OffMeshLinkData linkData = navAgent.currentOffMeshLinkData;
    //        Vector3 startPos = linkData.startPos;
    //        Vector3 endPos = linkData.endPos;

    //        //점프 경로 계산 (포물선)
    //        float elapsedTime = 0;
    //        while (elapsedTime < jumpDuration)
    //        {
    //            float t = elapsedTime / jumpDuration;
    //            Vector3 currentPosition = Vector3.Lerp(startPos, endPos, t);
    //            currentPosition.y = Mathf.Sin(t * Mathf.PI) * jumpHeight;
    //            transform.position = currentPosition;

    //            elapsedTime += Time.deltaTime;
    //            yield return null;
    //        }

    //        //도착점의 위치
    //        transform.position = endPos;
    //        //네브메쉬 경로 재개
    //        navAgent.CompleteOffMeshLink();
    //        navAgent.isStopped = false;
    //        isJumping = false;
    //    }

}
