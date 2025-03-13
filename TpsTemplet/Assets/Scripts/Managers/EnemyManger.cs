using UnityEngine;

//�̴� ���ӿ� �� ���� �ڵ�
//����� Ʈ���� ���� �׽�Ʈ�� �ڵ�� ���[������ ��Ȱ �������� �̵��ϴ� �̺�Ʈ] 2025- 03 - 12
//�̰����� �������� �ְ�, �޴� �׽�Ʈ ������. ���Ŀ��� ��Ai�� ��� 2025 03 13

//���´� �ִϸ��̼� ����
public enum enemyState
{
    idle,
    move,
    attack,
    panic,
    death,
    evade,       //ȸ�� - �ִϸ��̼��� move ���
    patroll,     //���� - move ���
    track       // ���� - move ���
}

public class EnemyManger : MonoBehaviour
{
    public enemyState currentState = enemyState.idle;
    public Transform target;        //Ÿ���� �÷��̾�
    public float attackRange = 1.0f;    //���� ����. ���� �� ������ ���� ����
    public float attackDelay = 2.0f;    //���� ������
    public float moveSpeed = 2.0f;      //�̵� �ӵ�
    private float nextAttackTime = 0.0f;    //���� ���� �ð� ����
    public Transform[] patrolPoints;        //���� ��� ������
    private int currentPoint = 0;       //���� ���� ��� ���� �ε���
    private float targetRange = 3.5f;      // ���� ����
    private bool isAttack = false;          //���� ����
    private float evadeRange = 5.0f;      //ȸ�� ����

    private float sweeperHP = 100.0f;    //����� �������� ü��
    private float distanceToTarget;     //Ÿ�ٰ��� �Ÿ� ��� ��
    private bool isWaiting = false;     //���� ��ȯ �� ��� ���� ����
    public float idleTime = 2.0f;       //�� ���� ��ȯ �� ��� �ð�







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


            Debug.Log("�Ÿ�  " + distanceToTarget);
        }

        else if (distanceToTarget < attackRange)
        {
            //Debug.Log("���� ����");
        }
        else
        {
            if (patrolPoints.Length > 0)
            {
                Debug.Log("������");
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
