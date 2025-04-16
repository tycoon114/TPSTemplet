using System;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public class CharacterSpawnManager : NetworkBehaviour
{
    public static event Action<CharacterInfo> OnLoadCharacterData;

    private Transform player;
    private GameObject selectedCharacter;
    public GameObject[] characterPrefabs;
    //private void Awake()
    //{
    //    player = GameObject.Find("Player").GetComponent<Transform>();
    //    int selectedID = CharacterSelectManager.Instance.selectedCharacterIndex;
    //    if (selectedID < 0 || selectedID >= characterPrefabs.Length)
    //    {
    //        Debug.LogError("잘못된 캐릭터 ID입니다.");
    //        return;
    //    }
    //    Debug.Log(selectedID);
    //    selectedCharacter = Instantiate(characterPrefabs[selectedID], player.position, Quaternion.identity);
    //    selectedCharacter.transform.SetParent(player);
    //    selectedCharacter.name = characterPrefabs[selectedID].name;

    //    LoadCharacterData(selectedCharacter.name, selectedCharacter);
    //}
    void Awake()
    {
        Debug.Log("캐릭터 스폰 매니저 시작");
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton 이 null 입니다. 확인 필요.");
            return;
        }

        // ConnectionApprovalCallback은 1개만 등록 가능하므로 덮어쓰기 방식
        if (NetworkManager.Singleton.ConnectionApprovalCallback == null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        }
        else
        {
            Debug.LogWarning("ConnectionApprovalCallback 이 이미 등록되어 있음");
        }

        // OnClientConnectedCallback은 이벤트이므로 -= 후 +=
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        // 아직 호스트가 아니라면 여기서 시작
        if (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
        {
            int selectedIndex = CharacterSelectManager.Instance.selectedCharacterIndex;
            Debug.Log(selectedIndex);
            NetworkManager.Singleton.NetworkConfig.ConnectionData =
                System.Text.Encoding.ASCII.GetBytes(selectedIndex.ToString());

            NetworkManager.Singleton.StartHost();
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        string payloadStr = System.Text.Encoding.ASCII.GetString(request.Payload);
        int selectedIndex = int.Parse(payloadStr);
        Debug.Log("ApprovalCheck TT   "  + selectedIndex);

        PlayerPrefs.SetInt("ApprovedCharacterIndex", selectedIndex); // 임시 저장
        response.Approved = true;
        response.CreatePlayerObject = false;
    }

    private void OnClientConnected(ulong clientId)
    {
        int selectedIndex = PlayerPrefs.GetInt("ApprovedCharacterIndex", 0);
        Debug.Log("OnClientConnected TT   " + selectedIndex);

        //GameObject prefab = characterPrefabs[selectedIndex];
        //Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(63, 73), 1, UnityEngine.Random.Range(320, 330));

        //GameObject character = Instantiate(prefab, spawnPos, Quaternion.identity);
        //character.name = prefab.name;
        //character.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        //Debug.Log("OnClientConnected TT   " + character.name);

        //// JSON 데이터 로딩
        //LoadCharacterData(character.name, character);
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

