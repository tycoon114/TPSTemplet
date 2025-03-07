using UnityEngine;


//카메라 컨트롤러 구버전
public class CameraController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0.5f, -4);// 카메라 위치 조정
    public float cameraSpeed = 5f; // 카메라 이동 속도
    public Transform player; // 플레이어 캐릭터
    public float sensitivity = 2.0f; // 마우스 감도
    public float zoomFOV = 10f; // 줌 시 FOV
    public float normalFOV = 20f; // 기본 FOV

    private float pitch = 0f; // 위아래 회전
    private float yaw = 0f; // 좌우 회전
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        //// 목표 위치 계산
        Vector3 desiredPosition = player.position + offset;

        //// 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * cameraSpeed);

        //// 항상 캐릭터를 바라보게 설정 
        //따라 가게만 해야 되서 우선 주석 처리
        transform.LookAt(player);

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -30f, 60f);          // 위아래 각도 제한

        // 카메라 위치 및 회전 적용
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = player.position + rotation * offset;
        transform.LookAt(player.position);

        //// 우클릭 시 줌 (FOV 조절)
        //float targetFOV = Input.GetMouseButton(1) ? zoomFOV : normalFOV;
        //cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, targetFOV, Time.deltaTime * 100f); // 더 빠르게 줌 적용

    }
}
