using UnityEngine;

public class AimController : MonoBehaviour
{
    public Transform spineTransform;  // 상체 (Bip001 Spine)
    public Transform weaponTransform; // 무기 (Bip001_Weapon)
    public Transform muzzlePoint;     // 총구 위치
    public Camera mainCamera;         // 메인 카메라
    public float rotationSpeed = 5f;  // 회전 속도
    public float maxUpAngle = 30f;    // 위 최대 각도
    public float maxDownAngle = -30f; // 아래 최대 각도

    private Quaternion originalSpineRotation;  // 초기 상체 회전값
    private Quaternion originalWeaponRotation; // 초기 무기 회전값

    void Start()
    {
        if (!spineTransform) spineTransform = FindChildTransform(transform, "Bip001 Spine");
        if (!weaponTransform) weaponTransform = FindChildTransform(transform, "Bip001_Weapon");
        if (!muzzlePoint) muzzlePoint = FindChildTransform(transform, "fire_01"); // 총구 위치
        if (!mainCamera) mainCamera = Camera.main;

        originalSpineRotation = spineTransform.localRotation;
        originalWeaponRotation = weaponTransform.localRotation;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(1)) // 우클릭 (조준)
        {
            Aim();
        }
        else
        {
            spineTransform.localRotation = originalSpineRotation;
            weaponTransform.localRotation = originalWeaponRotation;
        }
    }

    void Aim()
    {
        Vector3 aimTarget = GetAimTarget();

        // 🎯 스파인(상체) 회전
        Vector3 spineDirection = aimTarget - spineTransform.position;
        Quaternion spineRotation = Quaternion.LookRotation(spineDirection);
        spineRotation = ClampRotation(spineRotation); // 🎯 최대 각도 제한 적용
        spineTransform.rotation = Quaternion.Slerp(spineTransform.rotation, spineRotation, Time.deltaTime * rotationSpeed);

        // 🎯 무기 회전
        Vector3 weaponDirection = aimTarget - weaponTransform.position;
        Quaternion weaponRotation = Quaternion.LookRotation(weaponDirection);
        weaponTransform.rotation = Quaternion.Slerp(weaponTransform.rotation, weaponRotation, Time.deltaTime * rotationSpeed);
    }

    // 🎯 총구에서 레이캐스트를 쏴서 조준할 위치 반환
    Vector3 GetAimTarget()
    {
        Ray ray = new Ray(muzzlePoint.position, muzzlePoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            return hit.point;
        }
        else
        {
            return ray.origin + ray.direction * 100f;
        }
    }

    // 🎯 X축 회전 제한
    Quaternion ClampRotation(Quaternion rotation)
    {
        Vector3 euler = rotation.eulerAngles;
        euler.x = Mathf.Clamp(euler.x, maxDownAngle, maxUpAngle);
        return Quaternion.Euler(euler);
    }

    // 특정 이름을 가진 자식 오브젝트 찾기
    Transform FindChildTransform(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.name == name) return child;
        }
        return null;
    }
}
