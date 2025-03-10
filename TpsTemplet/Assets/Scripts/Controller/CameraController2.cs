using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public Vector3 CameraOffset = new Vector3(0, 0.5f, -4);   // ī�޶� ��ġ
    public float mouseSensitivity = 60.0f;             //���� - ���� �ɼǿ��� ���� �����ϰ�

    private float pitch = 0f;   // ���Ʒ� ȸ��
    private float yaw = 0f;     // �¿� ȸ��

    public Transform Player;        //�÷��̾��� ��ġ
    private Vector3 lookPosition;   //���� ��ġ?
    public Transform PlayerLookObj;  // �÷��̾� �� ī�޶� ���� ������Ʈ
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺 ���
        Cursor.visible = false;                   //Ŀ�� �Ⱥ��̰�
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
        pitch = Mathf.Clamp(pitch, -30f, 45f);
        Vector3 direction = PlayerLookObj.position + CameraOffset;

        //�÷��̾� ��ġ���� ���ݴ� ������ ���� �ڸ���� �����.
        lookPosition = new Vector3(PlayerLookObj.position.x , PlayerLookObj.position.y + CameraOffset.y, PlayerLookObj.position.z);
        transform.position = PlayerLookObj.position + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;

        //transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        transform.LookAt(lookPosition);

        //������ �ð�ȭ �ϱ� ����
        Vector3 rayDirection = transform.position - PlayerLookObj.transform.position;
        Debug.DrawRay(PlayerLookObj.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);
    }
}
