using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSpawnManager : NetworkBehaviour
{
    public static event Action<CharacterInfo> OnLoadCharacterData;

    private Transform player;
    private GameObject selectedCharacter;
    public GameObject[] characterPrefabs;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "DevRoomScene") return;

        if (!NetworkManager.Singleton.IsServer) return;
        Debug.Log("스폰 매니저 시작");

        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;

        // 호스트 자신도 여기서 처리
        HandleClientConnected(NetworkManager.Singleton.LocalClientId);


    }

    private void HandleClientConnected(ulong clientId)
    {
        string payload = System.Text.Encoding.ASCII.GetString(NetworkManager.Singleton.NetworkConfig.ConnectionData);
        int selectedIndex = CustomNetWorkManager.Instance.GetCharacterIndex(clientId);

        Debug.Log($"클라이언트 {clientId} 캐릭터 인덱스 {selectedIndex}");

        var prefab = characterPrefabs[selectedIndex];
        var spawnPos = new Vector3(UnityEngine.Random.Range(63, 73), 1, UnityEngine.Random.Range(320, 330));
        var character = Instantiate(prefab, spawnPos, Quaternion.identity);
        character.name = prefab.name;
        character.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        LoadCharacterData(prefab.name, character);
    }


    private void SpawnCharacter(ulong clientId)
    {
        Debug.Log("들어왔나?");

        string data = System.Text.Encoding.ASCII.GetString(NetworkManager.Singleton.NetworkConfig.ConnectionData);
        int index = int.TryParse(data, out var i) ? i : 0;
        Debug.Log("ㅁㅁㅁㅁ   " + index);

        var prefab = characterPrefabs[index];
        var spawnPos = new Vector3(UnityEngine.Random.Range(63, 73), 1, UnityEngine.Random.Range(320, 330));
        var character = Instantiate(prefab, spawnPos, Quaternion.identity);

        character.name = prefab.name;
        character.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        // JSON 데이터 로딩
        LoadCharacterData(prefab.name, character);
    }


    //void Awake()
    //{
    //    Debug.Log("캐릭터 스폰 매니저 시작");
    //    if (NetworkManager.Singleton == null)
    //    {
    //        Debug.LogError("NetworkManager.Singleton 이 null 입니다. 확인 필요.");
    //        return;
    //    }

    //    // ConnectionApprovalCallback은 1개만 등록 가능하므로 덮어쓰기 방식
    //    if (NetworkManager.Singleton.ConnectionApprovalCallback == null)
    //    {
    //        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("ConnectionApprovalCallback 이 이미 등록되어 있음");
    //    }

    //    // OnClientConnectedCallback은 이벤트이므로 -= 후 +=
    //    NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    //    NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

    //    // 아직 호스트가 아니라면 여기서 시작
    //    //if (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
    //    //{
    //    //    int selectedIndex = CharacterSelectManager.Instance.selectedCharacterIndex;
    //    //    Debug.Log(selectedIndex);
    //    //    NetworkManager.Singleton.NetworkConfig.ConnectionData =
    //    //        System.Text.Encoding.ASCII.GetBytes(selectedIndex.ToString());

    //    //    NetworkManager.Singleton.StartHost();
    //    //}
    //}

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        string payloadStr = System.Text.Encoding.ASCII.GetString(request.Payload);
        int selectedIndex = int.Parse(payloadStr);
        Debug.Log("ApprovalCheck TT   "  + selectedIndex);

        PlayerPrefs.SetInt("ApprovedCharacterIndex", selectedIndex); // 임시 저장
        response.Approved = true;
        response.CreatePlayerObject = false;
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
                OnLoadCharacterData?.Invoke(info);
                return;
            }
        }
        Debug.LogError($"캐릭터 '{characterName}' 정보 없음 -> 이름 확인.");
    }

}

