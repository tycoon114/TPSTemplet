using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CustomNetWorkManager : MonoBehaviour
{

    //호스트의 서버에 대한 코드 
    //


    public static event Action<CharacterInfo> OnLoadCharacterData;

    public GameObject[] characterPrefabs;
    private Dictionary<ulong, int> clientSelections = new();
    private HashSet<ulong> spawnedClients = new();

    public static CustomNetWorkManager Instance
    {
        get;
        private set;
    }

    private void Awake()
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
    }

    void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;

            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        Debug.Log("ApprovalCheck");

        string payloadStr = System.Text.Encoding.ASCII.GetString(request.Payload);
        int selectedIndex = int.TryParse(payloadStr, out var idx) ? idx : 0;

        clientSelections[request.ClientNetworkId] = selectedIndex;

        response.Approved = true;
        response.CreatePlayerObject = false;
    }

    private void OnClientConnected(ulong clientId)
    {

        if (spawnedClients.Contains(clientId)) return;
        spawnedClients.Add(clientId);

    }

    public int GetCharacterIndex(ulong clientId)
    {
        if (clientSelections.TryGetValue(clientId, out int index))
            return index;

        return 0;
    }
}

//캐릭터 데이터를 담을 곳
[System.Serializable]
public class CharacterInfo
{
    public string name;
    public int health;
    public float speed;
    public float damage;
    public string gunType;
    public string atkType;
    public string dfnType;
    public float bulletSpeed;
    public float fireRate;
    public float reloadTime;
    public int maxAmmo;
    public bool isAutoFire;
}

[System.Serializable]
public class CharacterData
{
    public CharacterInfo[] characters;
}
