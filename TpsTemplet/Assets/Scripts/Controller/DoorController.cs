using UnityEngine;

//문의 작동을 위한 스크립트
//본 게임에서는 쓰지 않을 수도 있다.
//여닫이문 뿐만아니라 위아래로 열리거나 자동문도 여기서 관리할 예정

public class DoorController : MonoBehaviour
{
    private bool isOpen = false;    //문이 열렸는지 확인하는 변수
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

        //문이 향하는 방향과 플레이어의 방향을 비교
        return doProduct > 0;
    }

    public bool Open(Transform player)
    {
        if (!isOpen)
        {
            isOpen = true;

            if (isPlayerInFront(player))
            {
                //문이 정방향으로 열림
                animator.SetTrigger("OpenForward");
                //LastOpenedForward = true;
            }
            else
            {
                //문이 역방향으로 열림
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
