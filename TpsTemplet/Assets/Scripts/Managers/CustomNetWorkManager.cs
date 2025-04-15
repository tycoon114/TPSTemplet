using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CustomNetWorkManager : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    private Dictionary<ulong, int> clientSelections = new();

    public static CustomNetWorkManager Instance
    {
        get;
        private set;
    }
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        Debug.Log("ApprovalChecj");

        string payloadStr = System.Text.Encoding.ASCII.GetString(request.Payload);
        int selectedIndex = int.Parse(payloadStr);

        clientSelections[request.ClientNetworkId] = selectedIndex;

        response.Approved = true;
        response.CreatePlayerObject = false;
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log("clientConected");
        if (!clientSelections.TryGetValue(clientId, out int selectedIndex))
        {
            selectedIndex = 0;
        }

        GameObject prefab = characterPrefabs[selectedIndex];
        Vector3 spawnPos = new Vector3(Random.Range(73, 63), 1, Random.Range(330, 320));
        GameObject playerObj = Instantiate(prefab, spawnPos, Quaternion.identity);
        playerObj.name = prefab.name;
        playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}
