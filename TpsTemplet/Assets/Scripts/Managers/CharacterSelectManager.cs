using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{

    private TextMeshProUGUI characterName;
    private Image characterImage;
    private string selectedCharacter;       //���̽� ���� �������� ���� ������ ĳ���� ���� ���� ����

    private void Awake()
    {
        characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        characterImage = GameObject.Find("CharacterImage").GetComponent<Image>();
    }

    public void OnKazusaClicked()
    {
        Debug.Log("Kazusa");
        selectedCharacter = "Kazusa";
        characterName.text = "ī���";
    }

    public void OnMariClicked()
    {
        Debug.Log("Mari");
        selectedCharacter = "Mari";
        characterName.text = "����";
    }

    public void OnNikoClicked()
    {
        Debug.Log("Niko");
        selectedCharacter = "Niko";
        characterName.text = "����";
    }

    public void OnShirokoClicked()
    {
        Debug.Log("Shiroko");
        selectedCharacter = "Shiroko";
        characterName.text = "�÷���";
    }


    public void OnCharacterConfirm()
    {

    }
}
