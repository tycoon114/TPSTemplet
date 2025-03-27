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
    }
}
