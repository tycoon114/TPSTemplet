using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Animations.Rigging;
using System.Security.Cryptography.X509Certificates;
using Unity.Netcode;
public class GunController : PlayerController
{
    public static event Action<int, int> onAmmoChanged;         //gamePlayUi에서 탄약을 표시 하기 위함
    public static event Action<bool> CrossHairSet;              //gamePlayUi에서 탄약을 표시 하기 위함

    //private Animator animator;
    private Transform muzzlePoint;                              //총구위치
    private Camera mainCamera;                                  //히트 스캔 레이캐스트를 위한 메인카메라 값

    private GameObject gunFire;                                  //사격 이펙트
    private GameObject impactEffect;                             //피격 이펙트

    public LayerMask hitLayers;                                 //맞출 수 있는 레이어

    private string gunType;                                     //총기 종류

    private int maxAmmo;                                        //최대 탄약수
    protected int currentAmmo;                                  //현재 잔탄 수

    private float bulletSpeed = 20f;                             //탄속
    private float fireRate = 0.2f;                               //연사 속도
    private float reloadTime;                                    //재장전 시간
    private float hitScanRadius = 0.05f;                         //크로스헤어 내 랜덤 범위
    private float range = 100f;                                  //사격 거리
    private float damage;                                       //공격력 - 변수 이름 나중에 바꿀 예정

    private bool isReload = false;                              //재장전
    private bool isShoot = false;                               //사격 애니메이션
    public bool boltAction = false;                             //볼트 액션이 아닌 경우 연사 가능하도록

    private Coroutine fireCoroutine;                            //연사 제어를 위한 코루틴 - 코루틴을 중지 시키기 위함[중지 시키지 않으면 연사속도가 중첩될 수 있음]
    public Coroutine cameraShakeCoroutine;                      //카메라 흔들림 코루틴

    //샷건
    private float shotGunSpreadAngle = 3.0f;                     //샷건 탄퍼짐 각도
    public float recoilStrength = 2.0f;                         //
    public float maxRecoilAngle = 10.0f;                        //
    public float currentRecoil = 0.0f;                          //
    public float shakeDuration = 0.1f;                          //
    public float shakeMagnitude = 0.1f;                         //
    public Vector3 originalCameraPosition;                      //원래 카메라 위치

    private PlayerController playerController;

    void Start()
    {
        if (characterInfo != null)
        {
            SetCharacterData(characterInfo);
        }
        Debug.Log(characterInfo);
        currentAmmo = maxAmmo;
        onAmmoChanged?.Invoke(currentAmmo, maxAmmo); // 탄약 UI 업데이트
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
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
        target = GameObject.Find("target").GetComponentInChildren<Transform>();
        hitLayers = LayerMask.GetMask("Wall", "Enemy", "Player", "EnemyPlayer");
    }

    private void SetCharacterData(CharacterInfo info)
    {
        Debug.Log(info.gunType);

        gunType = info.gunType;
        bulletSpeed = info.bulletSpeed;
        fireRate = info.fireRate;
        reloadTime = info.reloadTime;
        maxAmmo = info.maxAmmo;
        damage = info.damage;
    }

    void Update()
    {
        //스킬 애니메이션이 재생 중이면 return
        if (playerController != null && playerController.isSkillPlaying)
        {
            return;
        }
        Vector3 dir = transform.position + muzzlePoint.transform.position;

        if (Input.GetMouseButton(1))
        {
            isAim = true;
            CrossHairSet?.Invoke(isAim);
            //animator.SetLayerWeight(1, 0.7f);
        }
        else
        {
            isAim = false;
            CrossHairSet?.Invoke(isAim);
            //animator.SetLayerWeight(1, 0f);
        }
        animator.SetBool("isAim", isAim);
        //animator.SetLayerWeight(1, 1);

        if (Input.GetMouseButtonDown(0) && isAim && currentAmmo != 0 && fireCoroutine == null && !isReload && !IsInAnimationState("Shoot") && !IsInAnimationState("ShootDelay")) // && !IsInAnimationState("Shoot") -> 사격 애니메이션 중이면 사격 불가능..약간 수정 필요
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
            if (gunType.Equals("SG"))
            {
                ShootSG();
            }
            else
            {
                Shoot();
            }
            yield return new WaitForSeconds(fireRate);
        }
        yield return null;
    }
    IEnumerator Reload()
    {
        currentAmmo = maxAmmo;
        if (gunType.Equals("HG"))
        {
            SoundManager.Instance.PlayGunSfx("HGReload", target.transform.position);
        }
        else if (gunType.Equals("SMG"))
        {
            SoundManager.Instance.PlayGunSfx("SMGReload", target.transform.position);
        }
        else if (gunType.Equals("AR"))
        {
            SoundManager.Instance.PlayGunSfx("ARReload", target.transform.position);
        }
        else if (gunType.Equals("SR"))
        {
            SoundManager.Instance.PlayGunSfx("SRReload", target.transform.position);
        }
        else if (gunType.Equals("MG"))
        {
            SoundManager.Instance.PlayGunSfx("kazusaReload", target.transform.position);
        }
        else if (gunType.Equals("SG"))
        {
            SoundManager.Instance.PlayGunSfx("SGReload", target.transform.position);
        }

        isReload = true;
        animator.SetTrigger("isReload");

        yield return new WaitForSeconds(reloadTime);
        isReload = false;
        onAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }

    void Shoot()
    {
        //소리 재생 - 함수나 코루틴으로 뺄 예정
        if (gunType.Equals("HG"))
        {
            SoundManager.Instance.PlayGunSfx("HGShooting", target.transform.position);
        }
        else if (gunType.Equals("SMG"))
        {
            SoundManager.Instance.PlayGunSfx("SMGShooting", target.transform.position);
        }
        else if (gunType.Equals("AR"))
        {
            SoundManager.Instance.PlayGunSfx("ARShooting", target.transform.position);
        }
        else if (gunType.Equals("SR"))
        {
            SoundManager.Instance.PlayGunSfx("SRShooting", target.transform.position);
        }
        else if (gunType.Equals("MG"))
        {
            SoundManager.Instance.PlayGunSfx("MGShooting", target.transform.position);
        }

        //사격 이펙트
        if (gunFire != null)
        {
            //ParticleManager.Instance.PlayGunFireParticle("gunFire", muzzlePoint.position + muzzlePoint.forward * 0.1f, Vector3.one, muzzlePoint.rotation * Quaternion.Euler(90, -90, 0));
        }

        // 크로스헤어 중심에서 랜덤한 위치 생성 (히트스캔 범위 내)
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * hitScanRadius;
        Vector3 screenPoint = new Vector3(Screen.width / 2 + randomOffset.x, Screen.height / 2 + randomOffset.y, 0);
        Ray ray = mainCamera.ScreenPointToRay(screenPoint);

        //피격 판정이 있는 레이어 - hit.point : 충돌한 위치 : 파티클이 생길 위치
        if (Physics.Raycast(ray, out RaycastHit hit, range, hitLayers))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 3.0f);

            GameObject hitObject = hit.collider.gameObject;
            int hitLayer = hitObject.layer;

            Debug.Log($"감지된 오브젝트: {hitObject.name}, 레이어: {LayerMask.LayerToName(hitLayer)}");
            // 벽(Wall)과 충돌한 경우 피격 이펙트 생성
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.Log("wall 피격");
                ParticleManager.Instance.PlayGunHitParticle("hitWall", hit.point, Vector3.one, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                //미니게임용 - AI 한테 적용할 레이어다
                //hit.collider.GetComponent<EnemyManger>().TakeDamage(damage);
                Debug.Log("적을 맞춤");
            }
            else if (hit.collider.CompareTag("Player"))
            {
                //추후 멀티 활성화 시 아군인지 적팀인지 구분 필요
                Debug.Log("플레이어 피격");
            }
            else if (hitLayer == LayerMask.NameToLayer("EnemyPlayer"))
            {
                Debug.Log(" 레이어 피격 테스트");
                hit.collider.GetComponentInChildren<PlayerManager>().TakeDamage(damage);
                //var playerManager = hit.collider.GetComponentInChildren<PlayerManager>();

                //if (playerManager != null && !playerManager.GetComponent<NetworkObject>().IsOwner)
                //{
                //    playerManager.TakeDamage(damage);
                //}

            }
        }

        currentAmmo--;
        isShoot = true;
        animator.SetBool("isShoot", isShoot);
        onAmmoChanged?.Invoke(currentAmmo, maxAmmo); // 탄약 UI 업데이트
    }

    void ShootSG()
    {
        SoundManager.Instance.PlayGunSfx("SGShooting", target.transform.position);
        //샷건의 산탄은 5발로 - 변수로 빠질 수 있음
        for (int i = 0; i < 5; i++)
        {
            if (gunFire != null)
            {
                ParticleManager.Instance.PlayGunFireParticle("gunFire", muzzlePoint.position + muzzlePoint.forward * 0.1f, Vector3.one, muzzlePoint.rotation * Quaternion.Euler(90, -90, 0));
            }
            RaycastHit hit;

            Vector3 origin = Camera.main.transform.position;
            Vector3 spreadDirection = GetSpreadDirection(Camera.main.transform.forward, shotGunSpreadAngle);

            //Vector3 origin = muzzlePoint.position;
            //Vector3 spreadDirection = GetSpreadDirection(muzzlePoint.forward, shotGunSpreadAngle);

            Ray ray = mainCamera.ScreenPointToRay(spreadDirection);
            if (Physics.Raycast(origin, spreadDirection, out hit, range, hitLayers))
            {
                Debug.DrawLine(origin, hit.point, Color.green, 3.0f);
                if (hit.collider.CompareTag("Wall"))
                {
                    ParticleManager.Instance.PlayGunHitParticle("hitWall", hit.point, Vector3.one, Quaternion.LookRotation(hit.normal));
                }
            }
        }
        currentAmmo--;
        isShoot = true;
        animator.SetBool("isShoot", isShoot);
        onAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        SoundManager.Instance.PlayGunSfx("SGDelay", target.transform.position);
        animator.SetTrigger("isDelay");
    }

    Vector3 GetSpreadDirection(Vector3 forwardDirection, float spreadAngle)
    {
        float spreadX = UnityEngine.Random.Range(-spreadAngle, spreadAngle);
        float spreadY = UnityEngine.Random.Range(-spreadAngle, spreadAngle);
        Vector3 spreadDirection = Quaternion.Euler(spreadX, spreadY, 0) * forwardDirection;
        return spreadDirection;
    }

    //사격 애니메이션 중에는 사격 불가능, 현재 ShootDelay 상태가 없는 경우에만 제대로 작동함
    private bool IsInAnimationState(string animState)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animState);
    }

    //화면 흔들림
    private void gunShootShake()
    {

    }
}
