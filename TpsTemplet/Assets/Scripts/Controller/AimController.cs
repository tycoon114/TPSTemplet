using UnityEngine;

public class AimController : MonoBehaviour
{
    private Transform playerTransform; // 플레이어 Transform
    private Transform weaponTransform; // 무기 (Bip001_Weapon)
    private Transform spineTransform;  // 상체 (Bip001 Spine)

    public LayerMask aimLayerMask; // 조준 가능한 레이어 (지면, 벽, 적 등)

    public float maxSpineRotationX = 30f; // 상체 X축 최대 회전각
    public float maxSpineRotationY = 50f; // 상체 Y축 최대 회전각
    public float rotationSpeed = 10f;     // 회전 속도

    private bool isAiming = false;

    void Start()
    {
        playerTransform = GetComponentInParent<PlayerController2>()?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("PlayerController를 찾을 수 없습니다!");
            return;
        }

        // 🔎 "Bip001_Weapon"과 "Bip001 Spine"을 찾아서 할당
        Transform[] allChildren = playerTransform.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name == "Bip001_Weapon")
                weaponTransform = child;
            else if (child.name == "Bip001 Spine1")
                spineTransform = child;
        }

        //weaponTransform = playerTransform.Find("Bip001_Weapon");
        //spineTransform = playerTransform.Find("Bip001 Spine1");

        if (weaponTransform == null) Debug.LogError("무기를 찾을 수 없습니다! (Bip001_Weapon)");
        if (spineTransform == null) Debug.LogError("상체를 찾을 수 없습니다! (Bip001 Spine1)");

        Debug.Log("ㅁㄴㅁㅇㅁ " + weaponTransform.name);
        Debug.Log(spineTransform.name);
    }

    void LateUpdate() // 🎯 애니메이션 적용 후 회전 조정
    {


        if (spineTransform == null || weaponTransform == null) return;

        isAiming = Input.GetMouseButton(1); // 우클릭(조준)

        if (isAiming)
        {
            Debug.Log("Spine Rotation: " + spineTransform.rotation.eulerAngles);
            AimAtMouse();
        }
        else
        {
            // 조준 해제 시 원래 상태로 복귀
            spineTransform.localRotation = Quaternion.Slerp(spineTransform.localRotation, Quaternion.identity, Time.deltaTime * rotationSpeed);
        }
    }

    void AimAtMouse()
    {
        // 🔥 마우스가 가리키는 위치를 구하기 위한 레이캐스트
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimLayerMask))
        {
            Vector3 targetPoint = hit.point; // 마우스가 가리키는 3D 위치

            //무기 방향 회전
            Vector3 weaponDirection = (targetPoint - weaponTransform.position).normalized;
            Quaternion weaponRotation = Quaternion.LookRotation(weaponDirection);
            weaponTransform.rotation = Quaternion.Slerp(weaponTransform.rotation, weaponRotation, Time.deltaTime * rotationSpeed);

            //상체 방향 회전 (스파인)
            Vector3 spineDirection = (targetPoint - spineTransform.position).normalized;
            Quaternion spineRotation = Quaternion.LookRotation(spineDirection);

            // X축과 Y축 회전 제한
            float spineRotationX = Mathf.Clamp(spineRotation.eulerAngles.x, -maxSpineRotationX, maxSpineRotationX);
            float spineRotationY = Mathf.Clamp(spineRotation.eulerAngles.y, -maxSpineRotationY, maxSpineRotationY);

            //spineTransform.localRotation = Quaternion.Euler(spineRotationX, spineRotationY, 0);
        }
    }


}
