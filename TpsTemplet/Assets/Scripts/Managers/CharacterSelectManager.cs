using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{

    private TextMeshProUGUI characterName;
    private RawImage characterImage;
    private string selectedCharacter;       //���̽� ���� �������� ���� ������ ĳ���� ���� ���� ����
    private string path = "Image/portrait/Texture2D/Student_Portrait_";

    private void Awake()
    {
        characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        characterImage = GameObject.Find("CharacterImage").GetComponent<RawImage>();
    }

    public void OnKazusaClicked()
    {
        Debug.Log("Kazusa");
        selectedCharacter = "Kazusa";
        SetPortraitImage(selectedCharacter);
        characterName.text = "ī���";
    }

    public void OnMariClicked()
    {
        Debug.Log("Mari");
        selectedCharacter = "CH0186";
        SetPortraitImage(selectedCharacter);
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

    public void SetPortraitImage(string name)
    {
        string portraitPath = path + name;
        Texture2D characterPortrait = Resources.Load<Texture2D>(portraitPath);
        characterImage.texture = characterPortrait;
    }

}
