using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0.5f, -4);// 카메라 위치
    public float mouseSensitivity = 100.0f;  //감도

    private float pitch = 0f; // 위아래 회전
    private float yaw = 0f; // 좌우 회전

    //카메라는 LateUpdate를 사용 - 플레이어의 이동이 먼저 실행 되야되기 때문
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


    }
}
