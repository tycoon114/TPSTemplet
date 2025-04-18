using System;
using Unity.Netcode;
using UnityEngine;

//���Ӿ����� ��Ʈ��ũ�� ���� ����ȭ�� ���� �ڵ�
public class GameNetWorkManager : NetworkBehaviour
{
    public static GameNetWorkManager Instance;

    public static event Action<ulong, float> OnPlayerHit;

    void Awake()
    {
        Instance = this;
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerHitServerRpc(ulong playerId, float damage)
    {
        Debug.Log($"[����] {playerId} �� {damage} ������ ����");

        // �ǰ� �˸� (�̺�Ʈ �Ǵ� ���� ó��)
        OnPlayerHit?.Invoke(playerId, damage);

        // ������ ����� ��� Ŭ���̾�Ʈ���� ����
        ApplyDamageClientRpc(playerId, damage);
    }

    [ClientRpc]
    private void ApplyDamageClientRpc(ulong playerId, float damage)
    {
        if (NetworkManager.Singleton.LocalClientId == playerId)
        {
            //PlayerManager.Instance.TakeDamage(damage);
        }
    }
}
