using UnityEditor.Overlays;
using UnityEngine;


//카메라 컨트롤러 구버전
public class CameraControllerOld : MonoBehaviour
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


        //2025 03 10 - 카메라2 코드 옮겨둠
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //yaw += mouseX;
        //pitch += mouseY;
        ////위 아래 각도 제한
        //pitch = Mathf.Clamp(pitch, -30f, 45f);
        ////Vector3 direction = Player.position + CameraOffset;
        //Vector3 direction = PlayerLookObj.position + CameraOffset;

        ////부드럽게 움직이기
        ////transform.position = Vector3.SmoothDamp(transform.position, direction, ref mouseSensitivity, 0.3f);


        ////플레이어 위치에서 조금더 오른쪽 위로 자리잡게 만든다.
        ////lookPosition = new Vector3(Player.position.x + 0.5f, Player.position.y + CameraOffset.y, Player.position.z);
        //lookPosition = new Vector3(PlayerLookObj.position.x, PlayerLookObj.position.y + CameraOffset.y, PlayerLookObj.position.z);


        ////PlayerLookObj
        ////transform.position = Player.position  + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;
        //transform.position = PlayerLookObj.position + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;


        //transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        //transform.LookAt(lookPosition);

        ////광선을 시각화 하기 위함
        ////Vector3 rayDirection = transform.position - Player.transform.position;
        ////Debug.DrawRay(Player.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);

        //Vector3 rayDirection = transform.position - PlayerLookObj.transform.position;
        //Debug.DrawRay(PlayerLookObj.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);


        //2025-03-10  22:02 - smoothDemp 사용
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //yaw += mouseX;
        //pitch += mouseY;
        ////위 아래 각도 제한
        //pitch = Mathf.Clamp(pitch, -30f, 45f);
        //Vector3 direction = PlayerLookObj.position + CameraOffset;

        //float smoothPitch = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.x, pitch, ref pitchVelocity, smoothTime);
        //float smoothYaw = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, yaw, ref yawVelocity, smoothTime);
        //transform.rotation = Quaternion.Euler(smoothPitch, smoothYaw, 0);


        ////플레이어 위치에서 조금더 오른쪽 위로 자리잡게 만든다.
        ////lookPosition = new Vector3(PlayerLookObj.position.x , PlayerLookObj.position.y + CameraOffset.y, PlayerLookObj.position.z);
        //transform.position = PlayerLookObj.position + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;
        //lookPosition = new Vector3(PlayerLookObj.position.x, PlayerLookObj.position.y + CameraOffset.y, PlayerLookObj.position.z);
        //transform.LookAt(lookPosition);


        ////transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        //Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);
        ////transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);



        ////transform.LookAt(lookPosition);

        ////광선을 시각화 하기 위함
        //Vector3 rayDirection = transform.position - PlayerLookObj.transform.position;
        //Debug.DrawRay(PlayerLookObj.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);


    }
}
