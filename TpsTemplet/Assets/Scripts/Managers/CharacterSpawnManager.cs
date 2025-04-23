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

        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;

        // 호스트 자신도 여기서 처리
        HandleClientConnected(NetworkManager.Singleton.LocalClientId);
    }

    private void HandleClientConnected(ulong clientId)
    {
        string payload = System.Text.Encoding.ASCII.GetString(NetworkManager.Singleton.NetworkConfig.ConnectionData);
        //호스트가 방을 다시 만들었을 때 접속한 클라이언트의 캐릭터가 여러개 스폰 되는 것을 해결하는 코드
        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
        {
            var oldPlayerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            if (oldPlayerObject != null && oldPlayerObject.IsSpawned)
            {
                Debug.Log($"[SpawnManager] 기존 오브젝트 제거: {oldPlayerObject.name}");
                oldPlayerObject.Despawn(true);
            }
        }


        int selectedIndex = CustomNetWorkManager.Instance.GetCharacterIndex(clientId);

        Debug.Log($"플레이어 번호 {clientId} 캐릭터 인덱스 {selectedIndex}");

        var prefab = characterPrefabs[selectedIndex];
        var spawnPos = new Vector3(UnityEngine.Random.Range(63, 73), 1, UnityEngine.Random.Range(320, 330));
        var character = Instantiate(prefab, spawnPos, Quaternion.identity);
        character.name = prefab.name;
        character.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        LoadCharacterData(prefab.name, character);
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
                return;
            }
        }
        Debug.LogError($"캐릭터 '{characterName}' 정보 없음 -> 이름 확인.");
    }

}

