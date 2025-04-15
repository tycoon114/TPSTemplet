using System;
using Unity.Netcode;
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

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        int selectedID = CharacterSelectManager.Instance.selectedCharacterIndex;
        Debug.Log("Net   " + selectedID);
        GameObject characterPrefab = characterPrefabs[selectedID];
        GameObject character = Instantiate(characterPrefab, transform.position, Quaternion.identity);
        character.transform.SetParent(transform); // 내 NetworkObject 아래로 붙이기

        character.name = characterPrefab.name;

        LoadCharacterData(character.name, character);
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