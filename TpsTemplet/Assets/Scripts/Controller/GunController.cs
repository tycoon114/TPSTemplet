using System;
using UnityEngine;
using System.Collections;
//using System.Security.Claims;
//using static UnityEngine.EventSystems.EventTrigger;
//using UnityEngine.UI;


public class GunController : MonoBehaviour
{
    public static event Action<int, int> onAmmoChanged;  //gamePlayUi에서 탄약을 표시 하기 위함

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
 

    private bool isReload = false;      // 재장전
    private bool isShoot = false;       //사격 애니메이션
    private bool isAim = false;         //조준 여부 확인
    private Coroutine fireCoroutine;    // 연사 제어를 위한 코루틴 - 코루틴을 중지 시키기 위함[중지 시키지 않으면 연사속도가 중첩될 수 있음]

    public LayerMask hitLayers; // 맞출 수 있는 레이어

    private Camera mainCamera;      // 히트 스캔 레이캐스트를 위한 메인카메라 값

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
        hitLayers = LayerMask.GetMask("Wall", "Enemy" , "Player");
    }

   
    void Update()
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

        if (Input.GetMouseButtonDown(0) && isAim && currentAmmo!=0 && fireCoroutine == null && !isReload) // fire  ... 나중에 캐릭터 컨트롤러에서 여기를 오도록?
        {
            fireCoroutine = StartCoroutine(AttackStart());


        }
        else if (Input.GetMouseButtonUp(0) && fireCoroutine != null) // 좌클릭 해제 시 사격 중지
        {
            isShoot = false;
            //사격 애니메이션은 싱크가 맞지 않아 우선 비활성화
            //animator.SetBool("isShoot", isShoot);
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }

        if (Input.GetKeyDown(KeyCode.R) || currentAmmo ==0)
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
            StartCoroutine(Reload());
        }


    }

    IEnumerator AttackStart() {

        while (Input.GetMouseButton(0)) {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
        yield return null;
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
            // 벽(Wall)과 충돌한 경우 피격 이펙트 생성
            if (hit.collider.CompareTag("Wall"))
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                //미니게임용 - AI 한테 적용할 레이어다
                Debug.Log("적을 맞춤");
            }
            else if (hit.collider.CompareTag("Player"))
            {
                //추후 멀티 활성화 시 아군인지 적팀인지 구분 필요
            }
        }

        currentAmmo--;
        //isShoot = true;
        //animator.SetBool("isShoot",isShoot);
        onAmmoChanged?.Invoke(currentAmmo, maxAmmo); // 탄약 UI 업데이트
    }



}
