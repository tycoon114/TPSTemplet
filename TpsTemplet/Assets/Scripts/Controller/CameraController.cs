using System.Security.Claims;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 CameraOffset = new Vector3(0, 0.5f, -4);   // ī�޶� ������
    public float mouseSensitivity = 60.0f;             //���� - ���� �ɼǿ��� ���� �����ϰ�

    private float pitch = 0f;   // ���Ʒ� ȸ��
    private float yaw = 0f;     // �¿� ȸ��

    public Transform Player;        //�÷��̾��� ��ġ, ĳ���� ������
    private GameObject PlayerGo;    //�÷��̾� �� ���� ������Ʈ

    private Vector3 lookPosition;   //���� ��ġ?
    public Transform PlayerLookObj;  // �÷��̾� �� ī�޶� ���� ������Ʈ

    private Vector3 targetCamPosition;    //ī�޶� ��ġ��

    public float zoomSpeed = 5.0f; // Ȯ����Ұ� �Ǵ� �ӵ�

    private void OnEnable()
    {
        PlayerController.OnIsAim += UpdateCameraOffset;
    }

    private void OnDisable()
    {
        PlayerController.OnIsAim -= UpdateCameraOffset;
    }

    void Start()
    {
        PlayerGo = GameObject.Find("Player");
        Player = PlayerGo.transform.GetChild(0);
        PlayerLookObj = Player.transform.Find("PlayerObj");
    }

    //���� ������ ��ġ�� �ٲ�� ���װ� ����
    //�̴� ī�޶� �÷��̾ ���󰡴°Ͱ� ī�޶� �����Ӱ� �����̴°� �� �ΰ��� ���ÿ� �Ͼ�� ����\
    //ī�޶� �����Ӱ� �����̴� ���� isAim�� false�� �� �Ŀ��� �����ϵ��� �ڵ带 �����غ� ����
    //�̶� �÷��̾�� ������ �������� �ϳ� �ΰ� �̰͵� ���� ȸ���ϸ� ���� �ٶ󺸵���(�ٸ� ������Ʈ �ڵ� ����)
    //�����̴� ������ ī�޶� ���� ��Ű�� ����� �غ��� - ��Ʈ����� ���?

    //ī�޶�� LateUpdate�� ��� - �÷��̾��� �̵��� ���� ���� �ǾߵǱ� ����
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch += mouseY;
        //�� �Ʒ� ���� ����
        pitch = Mathf.Clamp(pitch, -30f, 10f);
        Vector3 direction = PlayerLookObj.position + CameraOffset;

        //�÷��̾� ��ġ���� ���ݴ� ������ ���� �ڸ���� �����. - ī�޶��� ��ġ
        lookPosition = new Vector3(PlayerLookObj.position.x, PlayerLookObj.position.y + CameraOffset.y, PlayerLookObj.position.z);
        transform.position = PlayerLookObj.position + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;



        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        transform.LookAt(lookPosition);
        //ī�޶� �� �浹
        avoidObjects();
    }

    void avoidObjects()
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.position - PlayerLookObj.transform.position;
        //������ �ð�ȭ �ϱ� ����
        Debug.DrawRay(PlayerLookObj.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);

        if (Physics.Raycast(PlayerLookObj.transform.position, rayDirection.normalized * rayDirection.magnitude, out hit, rayDirection.magnitude, LayerMask.GetMask("Wall")))
        {
            Debug.Log("Hit");
            //�ϴ� ���� ������ ������ ���� �ڵ�.... �߰������� �� �ٵ�� ��ߵ�
            transform.position = hit.point + hit.normal * 0.2f;
        }
        else
        {

        }

    }


    public void UpdateCameraOffset(bool isAim)
    {
        Vector3 targetOffset;

        if (isAim)
        {
            targetOffset = GetZoomedOffset();
        }
        else
        {
            targetOffset = GetDefaultOffset();
        }

        CameraOffset = Vector3.Lerp(CameraOffset, targetOffset, Time.deltaTime * zoomSpeed);
    }

    private Vector3 GetDefaultOffset()
    {
        return new Vector3(0, 0.5f, -4);
    }

    private Vector3 GetZoomedOffset()
    {
        return new Vector3(-1.9f, 0.7f, -1f);
    }


}



