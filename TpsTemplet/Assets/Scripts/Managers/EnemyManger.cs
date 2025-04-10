using System.Collections;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


//�̴� ���ӿ� �� ���� �ڵ�
//����� Ʈ���� ���� �׽�Ʈ�� �ڵ�� ���[������ ��Ȱ �������� �̵��ϴ� �̺�Ʈ] 2025- 03 - 12
//�̰����� �������� �ְ�, �޴� �׽�Ʈ ������. ���Ŀ��� ��Ai�� ��� 2025 03 13

//���´� �ִϸ��̼� ����
public enum enemyState
{
    idle,
    attack,
    panic,
    death,
    evade,       //ȸ�� - �ִϸ��̼��� move ���
    patroll,     //���� - move ���
    chase       // ���� - move ���
}

public class EnemyManger : MonoBehaviour
{
    //    public enemyState currentState = enemyState.idle;
    //    public float attackRange = 1.0f;    //���� ����. ���� �� ������ ���� ����
    //    public float attackDelay = 2.0f;    //���� ������
    //    public float moveSpeed = 2.0f;      //�̵� �ӵ�
    //    private float nextAttackTime = 0.0f;    //���� ���� �ð� ����
    //    public Transform[] patrolPoints;        //���� ��� ������
    //    private int currentPoint = 0;       //���� ���� ��� ���� �ε���
    //    private float trackingRange = 4.0f;      // ���� ����
    //    private bool isAttack = false;          //���� ����
    //    private float evadeRange = 5.0f;      //ȸ�� ����

    //    private float enemyHP = 100.0f;    //����� �������� ü��
    //    private float distanceToTarget;     //Ÿ�ٰ��� �Ÿ� ��� ��
    //    private bool isWaiting = false;     //���� ��ȯ �� ��� ���� ����
    //    public float idleTime = 2.0f;       //�� ���� ��ȯ �� ��� �ð�

    //    private Animator animator;

    //    private Coroutine stateRoutine;         //���� �������� �ڷ�ƾ

    //    private NavMeshAgent navAgent;

    //    private bool isJumping = false;     //onGround �� �ٲ�� ����
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
    //        //���� ���� ��� ����
    //        if (isJumping) return;

    //        if (stateRoutine != null)
    //        {
    //            Debug.Log("���� ����   " + stateRoutine + "   " + newState);
    //            StopCoroutine(stateRoutine);
    //        }
    //        currentState = newState;    //�ٲ��� ����

    //        //if������ ���� ����ġ���� �����
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
    //        Debug.Log(gameObject.name + "���");
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
    //        Debug.Log(gameObject.name + "����");

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
    //        Debug.Log(gameObject.name + "����");

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
    //        Debug.Log(gameObject.name + "����");


    //        navAgent.isStopped = true;
    //        navAgent.destination = PlayerManager.Instance.transform.position;


    //        animator.SetTrigger("attack");
    //        //transform.LookAt(target.position);
    //        //���� �ð��� ���� ���� �߰� ����
    //        yield return new WaitForSeconds(attackDelay);

    //        float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
    //        if (distance > attackRange)
    //        {
    //            //���� ������ ���� ���� ���
    //            ChangeState(enemyState.chase);
    //        }
    //        else
    //        {
    //            ChangeState(enemyState.attack);
    //        }


    //    }
    //    private IEnumerator Evade()
    //    {
    //        Debug.Log(gameObject.name + "����");
    //        animator.SetBool("isMove", true);

    //        Vector3 evadeDirection = (transform.position - PlayerManager.Instance.transform.position).normalized;
    //        float evadeTime = 3.0f;
    //        float timer = 0.0f;

    //        //�ٶ� ����� ���� ������ LookAt ��� ����

    //        //�� ������ while ���ȿ� �־ ������ �Ǳ��� - ��Ȳ�� ���� �°� ����
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
    //        Debug.Log(gameObject.name + "  ȥ��");

    //        yield return null;
    //    }
    //    private IEnumerator Death()
    //    {
    //        Debug.Log(gameObject.name + "  ���");
    //        animator.SetTrigger("isDeath");

    //        yield return new WaitForSeconds(2.0f);
    //        gameObject.SetActive(false);
    //    }




    //    public void TakeDamage(float damage)
    //    {
    //        Debug.Log(gameObject.name + "  ������ ����");

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
    //        //������Ʈ �ߴ�
    //        navAgent.isStopped = true;

    //        OffMeshLinkData linkData = navAgent.currentOffMeshLinkData;
    //        Vector3 startPos = linkData.startPos;
    //        Vector3 endPos = linkData.endPos;

    //        //���� ��� ��� (������)
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

    //        //�������� ��ġ
    //        transform.position = endPos;
    //        //�׺�޽� ��� �簳
    //        navAgent.CompleteOffMeshLink();
    //        navAgent.isStopped = false;
    //        isJumping = false;
    //    }

}
