using UnityEngine;

//문의 작동을 위한 스크립트
//본 게임에서는 쓰지 않을 수도 있다.
public class DoorController : MonoBehaviour
{
    private bool isOpen = false;    //문이 열렸는지 확인하는 변수
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
