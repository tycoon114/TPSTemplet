﻿using System.Security.Claims;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Vector3 currentLookOffset = Vector3.zero;
    public Vector3 CameraOffset = new Vector3(0, 0.5f, -4);   // 카메라 오프셋
    public float mouseSensitivity = 60.0f;             //감도 - 이후 옵션에서 조정 가능하게

    private float pitch = 0f;   // 위아래 회전
    private float yaw = 0f;     // 좌우 회전

    public Transform Player;        //플레이어의 위치, 캐릭터 프리펩
    private GameObject PlayerGo;    //플레이어 빈 게임 오브젝트

    private Vector3 lookPosition;   //보는 위치?
    public Transform PlayerLookObj;  // 플레이어 옆 카메라가 향할 오브젝트

    private Vector3 targetCamPosition;    //카메라 위치값

    public float zoomSpeed = 5.0f; // 확대축소가 되는 속도

    private bool isAiming = false;

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

    //카메라를 자유롭게 움직이는 것은 isAim이 false가 된 후에만 가능하도록 코드를 수정해볼 예장
    //이때 플레이어옆에 임의의 오프셋을 하나 두고 이것도 같이 회전하며 것을 바라보도록(다른 프로젝트 코드 참고)
    //움직이는 동안은 카메라를 고정 시키는 방법도 해볼것 - 스트리노바 방식?

    //카메라는 LateUpdate를 사용 - 플레이어의 이동이 먼저 실행 되야되기 때문
    void LateUpdate()
    {


        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch += mouseY;
        //위 아래 각도 제한
        pitch = Mathf.Clamp(pitch, -45f, 10f);


        if (isAiming)
        {
            Vector3 direction = PlayerLookObj.position + CameraOffset;

            //플레이어 위치에서 조금더 오른쪽 위로 자리잡게 만든다. - 카메라의 위치
            lookPosition = new Vector3(PlayerLookObj.position.x, PlayerLookObj.position.y + CameraOffset.y, PlayerLookObj.position.z);
            transform.position = PlayerLookObj.position + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;

            Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            transform.LookAt(lookPosition);

        }
        else
        {
            lookPosition = Player.position + new Vector3(0.62f, CameraOffset.y, 0);

            transform.position = Player.position + Quaternion.Euler(-pitch, yaw, 0) * CameraOffset;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(pitch, yaw, 0), Time.deltaTime * 5f);

            transform.LookAt(lookPosition);
        }
        //카메라 벽 충돌
        avoidObjects();
    }

    void avoidObjects()
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.position - PlayerLookObj.transform.position;
        //광선을 시각화 하기 위함
        Debug.DrawRay(PlayerLookObj.transform.position, rayDirection.normalized * rayDirection.magnitude, Color.red);

        if (Physics.Raycast(PlayerLookObj.transform.position, rayDirection.normalized * rayDirection.magnitude, out hit, rayDirection.magnitude, LayerMask.GetMask("Wall")))
        {
            Debug.Log("Hit");
            //일단 벽에 닿으면 가까이 가는 코드.... 추가적으로 더 다듬어 줘야됨
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
            isAiming = isAim;
            targetOffset = GetZoomedOffset();
        }
        else
        {
            isAiming = isAim;
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
        return new Vector3(0, 0.5f, -4);
    }
}



