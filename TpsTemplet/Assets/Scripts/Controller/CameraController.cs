using System.Security.Claims;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private GameObject PlayerGo;                                    //플레이어 빈 게임 오브젝트
    public Transform Player;                                        //플레이어의 위치, 캐릭터 프리펩

    private Vector3 lookPosition;                                   //보는 위치
    public Vector3 CameraOffset = new Vector3(0f, 0.5f, -4.0f);     //카메라 오프셋

    private float pitch = 0f;                                       //위아래 회전
    private float yaw = 0f;                                         //좌우 회전
    private float defaultFov = 40f;                                 //기본 시야각
    private float zoomFov = 20f;                                    //줌 시야각
    public float zoomSpeed = 5.0f;                                  //확대축소가 되는 속도
    public float mouseSensitivity = 60.0f;                          //감도 - 이후 옵션에서 조정 가능하게

    private bool isAiming = false;
    private bool isFreeCamera = false;

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
    }

    //카메라는 LateUpdate를 사용 - 플레이어의 이동이 먼저 실행 되야되기 때문
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch += mouseY;
        //위 아래 각도 제한
        pitch = Mathf.Clamp(pitch, -45f, 10f);

        //시네머신의 screen Position처럼 화면을 돌림
        Quaternion cameraRotation = Quaternion.Euler(-pitch, yaw, 0);

        transform.position = Player.position + cameraRotation * CameraOffset;

        Vector3 offset = cameraRotation * new Vector3(0.6f, 0f, 0f);
        lookPosition = Player.position + new Vector3(0, CameraOffset.y, 0) + offset;

        transform.LookAt(lookPosition);

        //카메라 벽 충돌 체크
        avoidObjects();
    }

    void avoidObjects()
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.position - Player.transform.position;

        float cameraDistance = rayDirection.magnitude;

        //광선을 시각화 하기 위함
        Debug.DrawRay(Player.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);

        if (Physics.Raycast(Player.transform.position, rayDirection.normalized * rayDirection.magnitude, out hit, rayDirection.magnitude, LayerMask.GetMask("Wall")))
        {
            //일단 벽에 닿으면 가까이 가는 코드.... 추가적으로 더 다듬어 줘야됨
            transform.position = hit.point + hit.normal * 0.2f;
        }
        else
        {

        }
    }

    public void UpdateCameraOffset(bool isAim)
    {
        //조준 시 줌
        if (isAim)
        {
            isAiming = isAim;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomFov, Time.deltaTime * zoomSpeed);
        }
        else
        {
            isAiming = isAim;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, defaultFov, Time.deltaTime * zoomSpeed);
        }
    }
}