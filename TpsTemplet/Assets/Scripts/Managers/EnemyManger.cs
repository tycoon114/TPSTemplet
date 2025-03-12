using UnityEngine;

//�̴� ���ӿ� �� ���� �ڵ�
//����� Ʈ���� ���� �׽�Ʈ�� �ڵ�� ���[������ ��Ȱ �������� �̵��ϴ� �̺�Ʈ] 2025- 03 - 12
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
        //�ִϸ����͸� ������? -> ���� ������ �ִϸ��̼��� ��� ��ų �� �ִ�.
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
