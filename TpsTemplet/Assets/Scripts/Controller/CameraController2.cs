using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0.5f, -4);// ī�޶� ��ġ
    public float mouseSensitivity = 100.0f;  //����

    private float pitch = 0f; // ���Ʒ� ȸ��
    private float yaw = 0f; // �¿� ȸ��

    //ī�޶�� LateUpdate�� ��� - �÷��̾��� �̵��� ���� ���� �ǾߵǱ� ����
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


    }
}
