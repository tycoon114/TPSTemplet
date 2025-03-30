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
            Debug.LogError("잘못된 캐릭터 ID입니다.");
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
                Debug.Log($"이름: {info.name}, 체력: {info.health}, 속도: {info.speed}, 공격력: {info.attack} , 총기: {info.gunType}");
                //ApplyCharacterStats(character, info);
                return;
            }
        }

        Debug.LogError($"캐릭터 '{characterName}' 정보 없음 -> 이름 확인.");
    }

    private void ApplyCharacterStats(GameObject character, CharacterInfo info)
    {
        CharacterController characterController = character.GetComponent<CharacterController>();

        if (characterController != null)
        {
    
        }
        else
        {
            Debug.LogError("캐릭터 컨트롤러를 찾을 수 없습니다.");
        }
    }

}




//캐릭터 데이터를 담을 곳
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