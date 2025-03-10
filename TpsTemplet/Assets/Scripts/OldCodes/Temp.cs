using UnityEngine;

public class Temp : MonoBehaviour
{
    //public Transform playerBody; // 캐릭터 본체
    //public Transform gunModel; // 총기 모델
    //private Transform muzzlePoint; // 총구 위치
    //public Camera mainCamera; // 메인 카메라
    //private bool isAiming = false; // 조준 여부

    //void Start()
    //{
    //    // 🔍 fire_01을 찾기 (깊숙한 곳까지 탐색)
    //    muzzlePoint = GetMuzzlePoint();
    //}

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(1)) // 우클릭(조준 시작)
    //    {
    //        isAiming = true;
    //    }
    //    if (Input.GetMouseButtonUp(1)) // 우클릭 해제
    //    {
    //        isAiming = false;
    //    }

    //    if (isAiming)
    //    {
    //        AimAtMouse();
    //    }
    //}

    //void AimAtMouse()
    //{
    //    // 🔍 마우스 위치에서 3D 월드 좌표 얻기
    //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    Vector3 aimTarget;
    //    if (Physics.Raycast(ray, out hit)) // 마우스가 어떤 오브젝트에 닿으면
    //    {
    //        aimTarget = hit.point; // 해당 위치를 조준
    //    }
    //    else
    //    {
    //        aimTarget = ray.origin + ray.direction * 50f; // 아무것도 없으면 먼 거리 조준
    //    }

    //    // 🔄 캐릭터 회전 (조준 방향을 바라보게)
    //    Vector3 lookDirection = (aimTarget - playerBody.position).normalized;
    //    lookDirection.y = 0; // Y축 회전 방지 (수평 회전만)
    //    playerBody.forward = lookDirection;

    //    // 🔫 총구 회전 (마우스를 바라보게)
    //    if (muzzlePoint != null)
    //    {
    //        muzzlePoint.LookAt(aimTarget);
    //    }
    //}

    //Transform GetMuzzlePoint()
    //{
    //    // 🔍 깊숙한 곳에서도 fire_01 찾기
    //    Transform[] allChildren = gunModel.GetComponentsInChildren<Transform>();
    //    foreach (Transform child in allChildren)
    //    {
    //        if (child.name == "fire_01")
    //        {
    //            return child;
    //        }
    //    }
    //    Debug.LogError("fire_01을 찾을 수 없습니다!");
    //    return null;
    //}
}



//    private Transform playerTransform; // 플레이어 Transform
//    private Transform weaponTransform; // 무기 (Bip001_Weapon)
//    private Transform spineTransform;  // 상체 (Bip001 Spine)

//    public LayerMask aimLayerMask; // 조준 가능한 레이어 (지면, 벽, 적 등)

//    public float maxSpineRotationX = 30f; // 상체 X축 최대 회전각
//    public float maxSpineRotationY = 50f; // 상체 Y축 최대 회전각
//    public float rotationSpeed = 10f;     // 회전 속도

//    private bool isAiming = false;

//    void Start()
//{
//    playerTransform = GetComponentInParent<PlayerController2>()?.transform;
//    if (playerTransform == null)
//    {
//        Debug.LogError("PlayerController를 찾을 수 없습니다!");
//        return;
//    }

//    // 🔎 "Bip001_Weapon"과 "Bip001 Spine"을 찾아서 할당
//    Transform[] allChildren = playerTransform.GetComponentsInChildren<Transform>();
//    foreach (Transform child in allChildren)
//    {
//        if (child.name == "Bip001_Weapon")
//            weaponTransform = child;
//        else if (child.name == "Bip001 Spine1")
//            spineTransform = child;
//    }

//    //weaponTransform = playerTransform.Find("Bip001_Weapon");
//    //spineTransform = playerTransform.Find("Bip001 Spine1");

//    if (weaponTransform == null) Debug.LogError("무기를 찾을 수 없습니다! (Bip001_Weapon)");
//    if (spineTransform == null) Debug.LogError("상체를 찾을 수 없습니다! (Bip001 Spine1)");

//    Debug.Log("ㅁㄴㅁㅇㅁ " + weaponTransform.name);
//    Debug.Log(spineTransform.name);
//}

//void LateUpdate() // 🎯 애니메이션 적용 후 회전 조정
//{


//    if (spineTransform == null || weaponTransform == null) return;

//    isAiming = Input.GetMouseButton(1); // 우클릭(조준)

//    if (isAiming)
//    {
//        Debug.Log("Spine Rotation: " + spineTransform.rotation.eulerAngles);
//        AimAtMouse();
//    }
//    else
//    {
//        // 조준 해제 시 원래 상태로 복귀
//        spineTransform.localRotation = Quaternion.Slerp(spineTransform.localRotation, Quaternion.identity, Time.deltaTime * rotationSpeed);
//    }
//}

//void AimAtMouse()
//{
//    // 🔥 마우스가 가리키는 위치를 구하기 위한 레이캐스트
//    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//    if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimLayerMask))
//    {
//        Vector3 targetPoint = hit.point; // 마우스가 가리키는 3D 위치

//        //무기 방향 회전
//        Vector3 weaponDirection = (targetPoint - weaponTransform.position).normalized;
//        Quaternion weaponRotation = Quaternion.LookRotation(weaponDirection);
//        weaponTransform.rotation = Quaternion.Slerp(weaponTransform.rotation, weaponRotation, Time.deltaTime * rotationSpeed);

//        //상체 방향 회전 (스파인)
//        Vector3 spineDirection = (targetPoint - spineTransform.position).normalized;
//        Quaternion spineRotation = Quaternion.LookRotation(spineDirection);

//        // X축과 Y축 회전 제한
//        float spineRotationX = Mathf.Clamp(spineRotation.eulerAngles.x, -maxSpineRotationX, maxSpineRotationX);
//        float spineRotationY = Mathf.Clamp(spineRotation.eulerAngles.y, -maxSpineRotationY, maxSpineRotationY);

//        //spineTransform.localRotation = Quaternion.Euler(spineRotationX, spineRotationY, 0);
//    }
//}

