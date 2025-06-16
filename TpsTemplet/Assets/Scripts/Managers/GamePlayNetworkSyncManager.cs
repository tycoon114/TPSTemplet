using Unity.Netcode;
using UnityEngine;



//��Ʈ��ũ �� ��ġ�̵�  ����ȭ ���� ���� ��ũ��Ʈ
public class GamePlayNetworkSyncManager : NetworkBehaviour
{
    private CharacterController controller;
    private Animator animator;

    //�̵� �ִϸ��̼� ����ȭ
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
            // ��: �������� 0.1 �̻��� �� �����̴� ������ ó��
            isMoving.Value = controller.velocity.magnitude > 0.1f;
        }
        else
        {
            animator.SetBool("isMoving", isMoving.Value);
            //animator.SetBool("isAim",);
        }
    }
}