using UnityEngine;

//���� �۵��� ���� ��ũ��Ʈ
//�� ���ӿ����� ���� ���� ���� �ִ�.
public class DoorController : MonoBehaviour
{
    private bool isOpen = false;    //���� ���ȴ��� Ȯ���ϴ� ����
    private Transform door;


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

}
