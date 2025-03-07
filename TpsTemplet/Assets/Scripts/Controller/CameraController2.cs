using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public Vector3 CameraOffset = new Vector3(0, 0.5f, -4);   // ī�޶� ��ġ
    public float mouseSensitivity = 60.0f;             //���� - ���� �ɼǿ��� ���� �����ϰ�

    private float pitch = 0f;   // ���Ʒ� ȸ��
    private float yaw = 0f;     // �¿� ȸ��

    public Transform Player;        //�÷��̾��� ��ġ
    private Vector3 lookPosition;   //���� ��ġ?

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺 ���
        Cursor.visible = false;                   //Ŀ�� �Ⱥ��̰�
    }

    //ī�޶�� LateUpdate�� ��� - �÷��̾��� �̵��� ���� ���� �ǾߵǱ� ����
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch += mouseY;
        //�� �Ʒ� ���� ����
        pitch = Mathf.Clamp(pitch, -10f, 45f);

        //�÷��̾� ��ġ���� ���ݴ� ������ ���� �ڸ���� �����.
        lookPosition = new Vector3(Player.position.x + 0.5f, Player.position.y + CameraOffset.y, Player.position.z);
        
        Vector3 direction = Player.position + CameraOffset;

        transform.position = Player.position  + Quaternion.Euler(pitch, yaw, 0) * CameraOffset;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.LookAt(lookPosition);

        //������ �ð�ȭ �ϱ� ����
        Vector3 rayDirection = transform.position - Player.transform.position;
        Debug.DrawRay(Player.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);
    }
}
