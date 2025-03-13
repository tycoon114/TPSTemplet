using UnityEngine;

//미니 게임용 적 관련 코드
//현재는 트리거 관련 테스트용 코드로 사용[죽으면 부활 지역으로 이동하는 이벤트] 2025- 03 - 12
//이것으로 데미지를 주고, 받는 테스트 용으로. 이후에는 적Ai로 사용 2025 03 13

//상태는 애니메이션 기준
public enum enemyState
{
    idle,
    move,
    attack,
    panic,
    death,
    evade,       //회피 - 애니메이션은 move 사용
    patroll,     //순찰 - move 사용
    track       // 추적 - move 사용
}

public class EnemyManger : MonoBehaviour
{
    public enemyState currentState = enemyState.idle;
    public Transform target;        //타겟은 플레이어
    public float attackRange = 1.0f;    //공격 범위. 이후 적 종류에 따라 변경
    public float attackDelay = 2.0f;    //공격 딜레이
    public float moveSpeed = 2.0f;      //이동 속도
    private float nextAttackTime = 0.0f;    //다음 공격 시간 관리
    public Transform[] patrolPoints;        //순찰 경로 지점들
    private int currentPoint = 0;       //현재 순찰 경로 지점 인덱스
    private float targetRange = 3.5f;      // 추적 범위
    private bool isAttack = false;          //공격 상태
    private float evadeRange = 5.0f;      //회피 범위

    private float sweeperHP = 100.0f;    //현재는 스위퍼의 체력
    private float distanceToTarget;     //타겟과의 거리 계산 값
    private bool isWaiting = false;     //상태 전환 후 대기 상태 여부
    public float idleTime = 2.0f;       //각 상태 전환 후 대기 시간







    void Start()
    {

    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget < targetRange && distanceToTarget > 0.5f)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(target.position);


            Debug.Log("거리  " + distanceToTarget);
        }

        else if (distanceToTarget < attackRange)
        {
            //Debug.Log("공격 범위");
        }
        else
        {
            if (patrolPoints.Length > 0)
            {
                Debug.Log("순찰중");
                Transform targetPoint = patrolPoints[currentPoint];
                Vector3 direction = (targetPoint.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.LookAt(targetPoint.position);
                if (Vector3.Distance(transform.position, targetPoint.position) < 3.0f)
                {
                    currentPoint = (currentPoint + 1) % patrolPoints.Length;
                }

            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    public void OnTriggerEnter(Collider other)
    {

    }
}
