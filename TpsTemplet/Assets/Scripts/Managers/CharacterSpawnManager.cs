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
    //        Debug.LogError("�߸��� ĳ���� ID�Դϴ�.");
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
        character.transform.SetParent(transform); // �� NetworkObject �Ʒ��� ���̱�

        character.name = characterPrefab.name;

        LoadCharacterData(character.name, character);
    }



    private void LoadCharacterData(string characterName, GameObject character)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("JsonData/characterData");
        if (jsonFile == null)
        {
            Debug.LogError("���� ����.");
            return;
        }

        CharacterData characterData = JsonUtility.FromJson<CharacterData>(jsonFile.text);
        if (characterData == null || characterData.characters == null)
        {
            Debug.LogError("�߸��� ������");
            return;
        }

        // ������ ĳ������ ������ ã�� ����
        foreach (CharacterInfo info in characterData.characters)
        {
            if (info.name == characterName)
            {
                Debug.Log($"�̸�: {info.name}, ü��: {info.health}, �ӵ�: {info.speed}, ���ݷ�: {info.damage} , �ѱ�: {info.gunType}");
                //Ȯ���� �Ǹ� �̺�Ʈ �߻� -> playerController�� ���� ���� �ֱ�
                OnLoadCharacterData?.Invoke(info);
                return;
            }
        }
        Debug.LogError($"ĳ���� '{characterName}' ���� ���� -> �̸� Ȯ��.");
    }

}

//ĳ���� �����͸� ���� ��
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