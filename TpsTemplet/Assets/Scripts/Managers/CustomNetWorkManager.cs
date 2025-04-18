using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CustomNetWorkManager : MonoBehaviour
{

    //호스트의 서버에 대한 코드?
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
        //if (!NetworkManager.Singleton.IsServer) return;

        //if (!clientSelections.TryGetValue(clientId, out int selectedIndex))
        //{
        //    selectedIndex = 0;
        //    Debug.Log("테스트" + clientId);
        //}
        //Debug.Log("clientConected   " + selectedIndex);

        //GameObject prefab = characterPrefabs[selectedIndex];
        //Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(73, 63), 1, UnityEngine.Random.Range(330, 320));
        //GameObject playerObj = Instantiate(prefab, spawnPos, Quaternion.identity);
        //playerObj.name = prefab.name;
        //playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        //LoadCharacterData(prefab.name, playerObj);

        if (spawnedClients.Contains(clientId)) return;
        spawnedClients.Add(clientId);

    }

    public int GetCharacterIndex(ulong clientId)
    {
        if (clientSelections.TryGetValue(clientId, out int index))
            return index;

        return 0;
    }

    private void LoadCharacterData(string characterName, GameObject character)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("JsonData/characterData");
        if (jsonFile == null)
        {
            Debug.LogError("파일 없음.");
            return;
        }

        CharacterData characterData = JsonUtility.FromJson<CharacterData>(jsonFile.text);
        if (characterData == null || characterData.characters == null)
        {
            Debug.LogError("잘못된 데이터");
            return;
        }

        // 선택한 캐릭터의 정보를 찾아 적용
        foreach (CharacterInfo info in characterData.characters)
        {
            if (info.name == characterName)
            {
                Debug.Log($"이름: {info.name}, 체력: {info.health}, 속도: {info.speed}, 공격력: {info.damage} , 총기: {info.gunType}");
                //확인이 되면 이벤트 발생 -> playerController로 정보 보내 주기
                var controller = character.GetComponent<PlayerController>();
                if (controller != null)
                {
                    controller.SetCharacterData(info); // 커스텀 메서드로 바로 넘기기
                }
                return;
            }
        }
        Debug.LogError($"캐릭터 '{characterName}' 정보 없음 -> 이름 확인.");
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
}

[System.Serializable]
public class CharacterData
{
    public CharacterInfo[] characters;
}
