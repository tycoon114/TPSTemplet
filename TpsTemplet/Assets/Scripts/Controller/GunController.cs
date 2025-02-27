using System;
using UnityEngine;
using System.Collections;
using System.Security.Claims;


public class GunController : MonoBehaviour
{

    private Animator animator;
    public GameObject bulletPrefab; // 총알
    private Transform muzzlePoint; //총구위치
    public float bulletSpeed = 20f;  // 이후 캐릭터의 총기마다 속도를 바꾸고 이를  플레이어 컨트롤러에서 받아오돌고 할것 -DB
    public float reloadTime = 2f;   // 재장전 시간
    public int MaxAmmo = 10;

    private bool isReload = false;
    private bool isShoot = false;
    private bool isAim = false;
    private int currentAmmo;



    void Start()
    {
        currentAmmo = MaxAmmo;
        animator = GetComponent<Animator>();
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
            isAim = true;
        }
        else
        {
            isAim = false;
        }
        animator.SetBool("isAim", isAim);



        Debug.DrawRay(muzzlePoint.transform.position, muzzlePoint.position, Color.red);
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
            //&&조건으로 총알이 없는 경우도 실행하도록 추가 필요

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
        currentAmmo = MaxAmmo;
    }

    void Shoot()
    {
        currentAmmo--;
        //isShoot = true;
        //animator.SetBool("isShoot",isShoot);
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = muzzlePoint.forward * bulletSpeed;
    }


}
