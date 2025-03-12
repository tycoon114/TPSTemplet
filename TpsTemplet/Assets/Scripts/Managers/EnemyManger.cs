using UnityEngine;

//미니 게임용 적 관련 코드
//현재는 트리거 관련 테스트용 코드로 사용[죽으면 부활 지역으로 이동하는 이벤트] 2025- 03 - 12
//

public class EnemyManger : MonoBehaviour
{

    public AudioClip tempAudio;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        GunController gunController = other.GetComponentInChildren<GunController>();
        //애니메이터를 받으면? -> 적에 맞으면 애니메이션을 재생 시킬 수 있다.
        Animator animator = other.GetComponentInChildren<Animator>();
        AudioSource audioSource = other.GetComponentInChildren<AudioSource>();
        PlayerController3 playerController3 = other.GetComponentInChildren<PlayerController3>();

        if (animator) {
            animator.SetTrigger("isDeath");
        }

        if (audioSource) {
            audioSource.PlayOneShot(tempAudio);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = Vector3.zero;
            //other.gameObject.SetActive(false);
            Debug.Log(other.gameObject.transform.position);
        }
    }
}
