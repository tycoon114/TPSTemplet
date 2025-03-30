using UnityEngine;

public class CharacterSpawnManager : MonoBehaviour
{
    private Transform player;       
    private GameObject selectedCharacter;
    public GameObject[] characterPrefabs;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        int selectedID = CharacterSelectManager.Instance.selectedCharacterIndex;
        if (selectedID < 0 || selectedID >= characterPrefabs.Length)
        {
            Debug.LogError("�߸��� ĳ���� ID�Դϴ�.");
            return;
        }
        Debug.Log(selectedID);
        selectedCharacter = Instantiate(characterPrefabs[selectedID], player.position, Quaternion.identity);
        selectedCharacter.transform.SetParent(player);
        selectedCharacter.name = characterPrefabs[selectedID].name;


        LoadCharacterData(selectedCharacter.name, selectedCharacter);
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
                Debug.Log($"�̸�: {info.name}, ü��: {info.health}, �ӵ�: {info.speed}, ���ݷ�: {info.attack} , �ѱ�: {info.gunType}");
                //ApplyCharacterStats(character, info);
                return;
            }
        }

        Debug.LogError($"ĳ���� '{characterName}' ���� ���� -> �̸� Ȯ��.");
    }

    private void ApplyCharacterStats(GameObject character, CharacterInfo info)
    {
        CharacterController characterController = character.GetComponent<CharacterController>();

        if (characterController != null)
        {
    
        }
        else
        {
            Debug.LogError("ĳ���� ��Ʈ�ѷ��� ã�� �� �����ϴ�.");
        }
    }

}




//ĳ���� �����͸� ���� ��
[System.Serializable]
public class CharacterInfo
{
    public string name;
    public int health;
    public float speed;
    public int attack;
    public string gunType;
}

[System.Serializable]
public class CharacterData
{
    public CharacterInfo[] characters;
}