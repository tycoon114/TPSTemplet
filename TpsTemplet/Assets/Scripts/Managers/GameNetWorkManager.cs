using System;
using Unity.Netcode;
using UnityEngine;

//게임씬에서 네트워크를 통한 동기화를 위한 코드
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
        Debug.Log($"[서버] {playerId} 가 {damage} 데미지 받음");

        // 피격 알림 (이벤트 또는 직접 처리)
        OnPlayerHit?.Invoke(playerId, damage);

        // 데미지 결과를 모든 클라이언트에게 전파
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
