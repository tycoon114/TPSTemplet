using UnityEngine;

//���� �۵��� ���� ��ũ��Ʈ
//�� ���ӿ����� ���� ���� ���� �ִ�.
//�����̹� �Ӹ��ƴ϶� ���Ʒ��� �����ų� �ڵ����� ���⼭ ������ ����

public class DoorController : MonoBehaviour
{
    private bool isOpen = false;    //���� ���ȴ��� Ȯ���ϴ� ����
    private Transform door;
    private Animator animator;

    void Start()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name == "01_low")
            {
                door = child;
                break;
            }
        }
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            door.Rotate(0, 0, 90);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            door.Rotate(0, 0, -90);
        }
    }


    public bool isPlayerInFront(Transform player)
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float doProduct = Vector3.Dot(transform.forward, toPlayer);

        //���� ���ϴ� ����� �÷��̾��� ������ ��
        return doProduct > 0;
    }

    public bool Open(Transform player)
    {
        if (!isOpen)
        {
            isOpen = true;

            if (isPlayerInFront(player))
            {
                //���� ���������� ����
                animator.SetTrigger("OpenForward");
                //LastOpenedForward = true;
            }
            else
            {
                //���� ���������� ����
                animator.SetTrigger("OpenBackward");
                //LastOpenedForward = false;
            }
            return true;
        }
        return false;
    }

    public void CloserForward(Transform player)
    {
        if (isOpen) { 
            isOpen = false;
            animator.SetTrigger("CloseForward");
        }
    }

    public void CloserBackward(Transform player)
    {
        if (isOpen)
        {
            isOpen = false;
            animator.SetTrigger("CloseBackward");
        }
    }

}
