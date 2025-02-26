using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab; // 총알
    private Transform muzzlePoint; //총구위치
    public float bulletSpeed = 20f;

    void Start()
    {
        //자식 오브젝트에서 총구인 fire_01찾기
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name == "fire_01")
            {
                muzzlePoint = child;
                break;
            }
        }

        if (muzzlePoint == null)
        {
            Debug.LogError($"{gameObject.name} 없음");
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0)) // fire  ... 나중에 캐릭터 컨트롤러에서 여기를 오도록?
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzlePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.linearVelocity = muzzlePoint.forward * bulletSpeed; 
        }
    }
}
