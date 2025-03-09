using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public Vector3 CameraOffset = new Vector3(0, 0.5f, -4);   // 카메라 위치
    public float mouseSensitivity = 60.0f;             //감도 - 이후 옵션에서 조정 가능하게

    private float pitch = 0f;   // 위아래 회전
    private float yaw = 0f;     // 좌우 회전

    public Transform Player;        //플레이어의 위치
    private Vector3 lookPosition;   //보는 위치?

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
        Cursor.visible = false;                   //커서 안보이게
    }

    //현재 조준점 위치가 바뀌는 버그가 있음
    //이는 카메라가 플레이어를 따라가는것과 카메라를 자유롭게 움직이는것 이 두개가 동시에 일어나기 때문\
    //카메라를 자유롭게 움직이는 것은 isAim이 false가 된 후에만 가능하도록 코드를 수정해볼 예장
    //이때 플레이어옆에 임의의 오프셋을 하나 두고 이것도 같이 회전하며 것을 바라보도록(다른 프로젝트 코드 참고)

    //카메라는 LateUpdate를 사용 - 플레이어의 이동이 먼저 실행 되야되기 때문
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch += mouseY;
        //위 아래 각도 제한
        pitch = Mathf.Clamp(pitch, -10f, 45f);

        //플레이어 위치에서 조금더 오른쪽 위로 자리잡게 만든다.
        lookPosition = new Vector3(Player.position.x + 0.5f, Player.position.y + CameraOffset.y, Player.position.z);
        
        Vector3 direction = Player.position + CameraOffset;

        transform.position = Player.position  + Quaternion.Euler(pitch, yaw, 0) * CameraOffset;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.LookAt(lookPosition);

        //광선을 시각화 하기 위함
        Vector3 rayDirection = transform.position - Player.transform.position;
        Debug.DrawRay(Player.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);
    }
}
