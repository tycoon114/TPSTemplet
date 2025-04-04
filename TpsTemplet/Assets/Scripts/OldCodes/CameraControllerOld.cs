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


//using System.Security.Claims;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class CameraController : MonoBehaviour
//{
//    public Vector3 CameraOffset = new Vector3(0, 0.5f, -4);   // 카메라 오프셋
//    public float mouseSensitivity = 60.0f;             //감도 - 이후 옵션에서 조정 가능하게

//    private float pitch = 0f;   // 위아래 회전
//    private float yaw = 0f;     // 좌우 회전

//    public Transform Player;        //플레이어의 위치, 캐릭터 프리펩
//    private GameObject PlayerGo;    //플레이어 빈 게임 오브젝트

//    private Vector3 lookPosition;   //보는 위치?
//    public Transform PlayerLookObj;  // 플레이어 옆 카메라가 향할 오브젝트

//    private Vector3 targetCamPosition;    //카메라 위치값

//    public float zoomSpeed = 5.0f; // 확대축소가 되는 속도

//    private bool isFreeCameraMode = false;

//    private void OnEnable()
//    {
//        PlayerController.OnIsAim += UpdateCameraOffset;
//    }

//    private void OnDisable()
//    {
//        PlayerController.OnIsAim -= UpdateCameraOffset;
//    }

//    void Start()
//    {
//        PlayerGo = GameObject.Find("Player");
//        Player = PlayerGo.transform.GetChild(0);
//        PlayerLookObj = Player.transform.Find("PlayerObj");
//    }

//    //카메라를 자유롭게 움직이는 것은 isAim이 false가 된 후에만 가능하도록 코드를 수정해볼 예장
//    //이때 플레이어옆에 임의의 오프셋을 하나 두고 이것도 같이 회전하며 것을 바라보도록(다른 프로젝트 코드 참고)
//    //움직이는 동안은 카메라를 고정 시키는 방법도 해볼것 - 스트리노바 방식?

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.L))
//        {
//            isFreeCameraMode = !isFreeCameraMode;
//        }
//    }

//    //카메라는 LateUpdate를 사용 - 플레이어의 이동이 먼저 실행 되야되기 때문
//    void LateUpdate()
//    {
//        if (isFreeCameraMode)
//        {
//            //자유 카메라 모드
//            SetFreeCameraMode();
//        }
//        else
//        {
//            //일반 카메라 모드 - 캐릭터를 따라감, 화면을 돌리면 그방향으로 캐릭터도 돌아감
//            SetFollowCameraMode();
//        }

//        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
//        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

//        yaw += mouseX;
//        pitch += mouseY;
//        //위 아래 각도 제한
//        pitch = Mathf.Clamp(pitch, -30f, 10f);
//        //Vector3 direction = PlayerLookObj.position + CameraOffset;

//        //플레이어 위치에서 조금더 오른쪽 위로 자리잡게 만든다. - 카메라의 위치
//        //lookPosition = new Vector3(PlayerLookObj.position.x, PlayerLookObj.position.y + CameraOffset.y, PlayerLookObj.position.z);
//        //transform.position = PlayerLookObj.position + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;

//        //Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);
//        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

//        lookPosition = Player.position + new Vector3(0.62f, CameraOffset.y, 0);

//        transform.position = Player.position + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;
//        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(pitch, yaw, 0), Time.deltaTime * 5f);

//        transform.LookAt(lookPosition);
//        //카메라 벽 충돌
//        avoidObjects();
//    }

//    void avoidObjects()
//    {
//        RaycastHit hit;
//        Vector3 rayDirection = transform.position - PlayerLookObj.transform.position;
//        //광선을 시각화 하기 위함
//        Debug.DrawRay(PlayerLookObj.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);

//        if (Physics.Raycast(PlayerLookObj.transform.position, rayDirection.normalized * rayDirection.magnitude, out hit, rayDirection.magnitude, LayerMask.GetMask("Wall")))
//        {
//            Debug.Log("Hit");
//            //일단 벽에 닿으면 가까이 가는 코드.... 추가적으로 더 다듬어 줘야됨
//            transform.position = hit.point + hit.normal * 0.2f;
//        }
//        else
//        {

//        }
//    }

//    private void SetFreeCameraMode()
//    {
//        //Debug.Log("프리카메라  " + isFreeCameraMode);
//    }

//    private void SetFollowCameraMode()
//    {
//        //Debug.Log("프리카메라  " + isFreeCameraMode);
//    }


//    public void UpdateCameraOffset(bool isAim)
//    {
//        Vector3 targetOffset;

//        if (isAim)
//        {
//            targetOffset = GetZoomedOffset();
//        }
//        else
//        {
//            targetOffset = GetDefaultOffset();
//        }
//        CameraOffset = Vector3.Lerp(CameraOffset, targetOffset, Time.deltaTime * zoomSpeed);
//    }

//    private Vector3 GetDefaultOffset()
//    {
//        return new Vector3(0, 0.5f, -4);
//    }

//    private Vector3 GetZoomedOffset()
//    {
//        return new Vector3(-1.9f, 0.7f, -1f);
//    }


//}