using Unity.Netcode;
using UnityEngine;



//네트워크 상 위치이동  동기화 등을 위한 스크립트
public class GamePlayNetworkSyncManager : NetworkBehaviour
{
    private CharacterController controller;
    private Animator animator;

    //이동 애니메이션 동기화
    private NetworkVariable<bool> isMoving = new NetworkVariable<bool>(
        writePerm: NetworkVariableWritePermission.Owner);

    private NetworkVariable<bool> isAim = new NetworkVariable<bool>(
        writePerm: NetworkVariableWritePermission.Owner);



    void Start()
    {
        controller = GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    [Rpc(SendTo.Everyone)]
    public void TriggerSkillRpc(string skillName)
    {
        SoundManager.Instance.PlaySkillSfx(skillName, transform.position);
        animator.SetTrigger("isSkill");
    }

    private void Update()
    {
        if (IsOwner)
        {
            // 예: 움직임이 0.1 이상일 때 움직이는 중으로 처리
            isMoving.Value = controller.velocity.magnitude > 0.1f;
        }
        else
        {
            animator.SetBool("isMoving", isMoving.Value);
            //animator.SetBool("isAim",);
        }
    }
}