using System;
using UnityEngine;
using System.Collections;
using System.Security.Claims;
using static UnityEngine.EventSystems.EventTrigger;


public class GunController : MonoBehaviour
{
    public static event Action<int, int> onAmmoChanged;  //gamePlayUi에서 탄약을 표시 하기 위함

    private Animator animator;
    public GameObject bulletPrefab; // 총알
    private Transform muzzlePoint; //총구위치
    public GameObject gunFire;
    public GameObject impactEffect; // 피격 이펙트

    public float bulletSpeed = 20f;  // 이후 캐릭터의 총기마다 속도를 바꾸고 이를  플레이어 컨트롤러에서 받아오돌고 할것 -DB
    public float reloadTime = 2f;   // 재장전 시간
    public int maxAmmo = 10;

    private bool isReload = false;
    private bool isShoot = false;
    private bool isAim = false;
    protected int currentAmmo;

    public float range = 100f; // 사격 거리
    public float hitScanRadius = 0.05f; // 크로스헤어 내 랜덤 범위
    public LayerMask hitLayers; // 맞출 수 있는 레이어

    private Camera mainCamera;

    void Start()
    {
        currentAmmo = maxAmmo;
        onAmmoChanged?.Invoke(currentAmmo, maxAmmo); // 탄약 UI 업데이트
        animator = GetComponent<Animator>();

        mainCamera = Camera.main;

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

    }

   
    void FixedUpdate()
    {
        Vector3 dir = transform.position + muzzlePoint.transform.position;

        if (Input.GetMouseButton(1))
        {
            isAim = true;
        }
        else
        {
            isAim = false;
        }
        animator.SetBool("isAim", isAim);

        if (Input.GetMouseButtonDown(0) && isAim && currentAmmo!=0) // fire  ... 나중에 캐릭터 컨트롤러에서 여기를 오도록?
        {
            Shoot();
        }
        else if (Input.GetMouseButtonUp(0)) // 좌클릭 해제 시 사격 중지
        {
            isShoot = false;
            //사격 애니메이션은 싱크가 맞지 않아 우선 비활성화
            //animator.SetBool("isShoot", isShoot);
        }

        if (Input.GetKeyDown(KeyCode.R) || currentAmmo ==0)
        {
            StartCoroutine(Reload());
        }


    }

    IEnumerator Reload()
    {
        isReload = true;
        animator.SetBool("isReload", isReload);

        yield return new WaitForSeconds(reloadTime);
        isReload = false;
        animator.SetBool("isReload", isReload);
        currentAmmo = maxAmmo;
        onAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }

    void Shoot()
    {
        //총구 이펙트
        if (gunFire != null)
        {
            GameObject gunEffect = Instantiate(gunFire, muzzlePoint.position, Quaternion.identity);
            Destroy(gunEffect, 0.5f);
        }

        // 크로스헤어 중심에서 랜덤한 위치 생성 (히트스캔 범위 내)
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * hitScanRadius;
        Vector3 screenPoint = new Vector3(Screen.width / 2 + randomOffset.x, Screen.height / 2 + randomOffset.y, 0);
        Ray ray = mainCamera.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, range, hitLayers))
        {
            Debug.Log($"Hit: {hit.collider.name}");

            // 벽(Wall)과 충돌한 경우 피격 이펙트 생성
            if (hit.collider.CompareTag("Wall"))
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            //// 적(Enemy)과 충돌한 경우 데미지 적용
            //if (hit.collider.TryGetComponent(out Enemy enemy))
            //{
            //    enemy.TakeDamage(10); // 예제 데미지 값
            //}
        }

        currentAmmo--;
        //isShoot = true;
        //animator.SetBool("isShoot",isShoot);
        onAmmoChanged?.Invoke(currentAmmo, maxAmmo); // 탄약 UI 업데이트
    }



}
