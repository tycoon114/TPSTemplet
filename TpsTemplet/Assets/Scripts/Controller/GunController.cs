using System;
using UnityEngine;
using System.Collections;

public class GunController : PlayerController
{
    public static event Action<int, int> onAmmoChanged;  //gamePlayUi에서 탄약을 표시 하기 위함
    public static event Action<bool> CrossHairSet;  //gamePlayUi에서 탄약을 표시 하기 위함

    private Animator animator;
    private Transform muzzlePoint;      //총구위치
    protected int currentAmmo;          //현재 잔탄 수

    //캐릭터마다 바뀔 수 있는값, 현재는 개발의 편의성을 위해 public 처리
    public GameObject gunFire;        //사격 이펙트
    public GameObject impactEffect; // 피격 이펙트
    public float bulletSpeed = 20f;  // 탄속
    public float fireRate = 0.2f;    //연사 속도
    public float reloadTime = 2f;   // 재장전 시간
    public float hitScanRadius = 0.05f; // 크로스헤어 내 랜덤 범위
    public float range = 100f; // 사격 거리
    public int maxAmmo = 50;        //최대 탄약수
    public bool boltAction = false;  //볼트 액션이 아닌 경우 연사 가능하도록
    public float damage = 30.0f;    //공격력 - 변수 이름 나중에 바꿀 예정
    public string gunType = "MG";   //이후 받아와서 처리

    private bool isReload = false;      // 재장전
    private bool isShoot = false;       //사격 애니메이션
    private Coroutine fireCoroutine;    // 연사 제어를 위한 코루틴 - 코루틴을 중지 시키기 위함[중지 시키지 않으면 연사속도가 중첩될 수 있음]

    public LayerMask hitLayers;         // 맞출 수 있는 레이어

    private Camera mainCamera;          // 히트 스캔 레이캐스트를 위한 메인카메라 값


    void Start()
    {
        currentAmmo = maxAmmo;
        onAmmoChanged?.Invoke(currentAmmo, maxAmmo); // 탄약 UI 업데이트
        animator = GetComponentInChildren<Animator>();

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
        hitLayers = LayerMask.GetMask("Wall", "Enemy", "Player");
    }


    void Update()
    {
        Vector3 dir = transform.position + muzzlePoint.transform.position;

        if (Input.GetMouseButton(1))
        {
            isAim = true;
            //UpdateAimTarget();
            CrossHairSet?.Invoke(isAim);
        }
        else
        {
            isAim = false;
            CrossHairSet?.Invoke(isAim);
        }
        animator.SetBool("isAim", isAim);
        //animator.SetLayerWeight(1, 1);

        if (Input.GetMouseButtonDown(0) && isAim && currentAmmo != 0 && fireCoroutine == null && !isReload) // fire  ... 나중에 캐릭터 컨트롤러에서 여기를 오도록?
        {
            fireCoroutine = StartCoroutine(AttackStart());
        }
        else if (Input.GetMouseButtonUp(0) && fireCoroutine != null) // 좌클릭 해제 시 사격 중지
        {
            isShoot = false;
            animator.SetBool("isShoot", isShoot);
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }

        if (Input.GetKeyDown(KeyCode.R) || currentAmmo == 0)
        {
            isShoot = false;
            animator.SetBool("isShoot", isShoot);

            //탄창량이 0인것을 계속 확인해서 0일때 재장전 모션이 잘 안나옴
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
            StartCoroutine(Reload());
        }
    }
    IEnumerator AttackStart()
    {
        while (Input.GetMouseButton(0))
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
        yield return null;
    }
    IEnumerator Reload()
    {
        //소리 겹침 다시 발생
        SoundManager.Instance.StopGunSfx();
        SoundManager.Instance.PlayGunSfx("kazusaReload", target.transform.position);

        isReload = true;
        animator.SetTrigger("isReload");

        yield return new WaitForSeconds(reloadTime);
        isReload = false;
        currentAmmo = maxAmmo;
        onAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }

    void Shoot()
    {
        //소리 재생 - 함수나 코루틴으로 뺄 예정
        if (gunType.Equals("HG"))
        {
            //SoundManager.Instance.PlayGunSfx("HGShooting", target.transform.position);
        }
        else if (gunType.Equals("SMG"))
        {
            //SoundManager.Instance.PlayGunSfx("SMGShooting", target.transform.position);
        }
        else if (gunType.Equals("AR"))
        {
            SoundManager.Instance.PlayGunSfx("ARShooting", target.transform.position);
        }
        else if (gunType.Equals("SR"))
        {
            //SoundManager.Instance.PlayGunSfx("SRhooting", target.transform.position);
        }
        else if (gunType.Equals("MG"))
        {
            SoundManager.Instance.PlayGunSfx("MGShooting", target.transform.position);
        }

        //사격 이펙트
        if (gunFire != null)
        {
            GameObject gunEffect = Instantiate(gunFire, muzzlePoint.position + muzzlePoint.forward * 0.1f, muzzlePoint.rotation * Quaternion.Euler(90, -90, 0));
            //ParticleManager.Instance.PariclePlay(ParticleType.GunFire, muzzlePoint.position + muzzlePoint.forward * 0.1f, Vector3.one, muzzlePoint.rotation * Quaternion.Euler(90, -90, 0));
            Destroy(gunEffect, 0.5f);
        }

        // 크로스헤어 중심에서 랜덤한 위치 생성 (히트스캔 범위 내)
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * hitScanRadius;
        Vector3 screenPoint = new Vector3(Screen.width / 2 + randomOffset.x, Screen.height / 2 + randomOffset.y, 0);
        Ray ray = mainCamera.ScreenPointToRay(screenPoint);

        //RaycastHit[] hits = Physics.RaycastAll(ray, range, hitLayers);

        //피격 판정이 있는 레이어 - hit.point : 충돌한 위치 : 파티클이 생길 위치
        if (Physics.Raycast(ray, out RaycastHit hit, range, hitLayers))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 3.0f);
            // 벽(Wall)과 충돌한 경우 피격 이펙트 생성
            if (hit.collider.CompareTag("Wall"))
            {
                ParticleManager.Instance.PariclePlay(ParticleType.GunHitWall, hit.point, Vector3.one, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                //미니게임용 - AI 한테 적용할 레이어다
                hit.collider.GetComponent<EnemyManger>().TakeDamage(damage);
                Debug.Log("적을 맞춤");
            }
            else if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("플레이어 피격");
                //추후 멀티 활성화 시 아군인지 적팀인지 구분 필요
            }
        }

        //여러개 - 관통 타입만 이 방식으로 사용할 예정 if(type == ....);
        //RaycastHit[] hits;
        //Ray rays = new Ray();
        //hits = Physics.RaycastAll(rays.origin, transform.forward, range, hitLayers);
        //if (hits.Length > 0)
        //{
        //    for (int i = 0; i < hits.Length &% i <2; i++)
        //    {
        //        RaycastHit hitt = hits[i];
        //        Debug.DrawLine(ray.origin, hit.point, Color.green, 3.0f);
        //    }
        //}

        currentAmmo--;
        isShoot = true;
        animator.SetBool("isShoot", isShoot);
        onAmmoChanged?.Invoke(currentAmmo, maxAmmo); // 탄약 UI 업데이트
    }
}
