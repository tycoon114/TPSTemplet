using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CharacterSelectManager : MonoBehaviour
{
    private TextMeshProUGUI characterName;
    private RawImage characterImage;
    public string selectedCharacter = "temp";       //���̽� ���� �������� ���� ������ ĳ���� ���� ���� ����
    private string path = "Image/portrait/Texture2D/Student_Portrait_";

    public int selectedCharacterIndex;

    //���� �׽�Ʈ�� �����ϰ� �ϱ� ���� �̱��� ���, ��Ƽ �÷��̷� �۾��� ���� ����Ƽ ���ڵ� ����� ��
    public static CharacterSelectManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        characterImage = GameObject.Find("CharacterImage").GetComponent<RawImage>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SelectCharacter(int index)
    {
        selectedCharacterIndex = index;
        //confirmButton.interactable = true;  // �����ϸ� Ȯ�� ��ư Ȱ��ȭ
    }

    public void OnKazusaClicked()
    {
        selectedCharacter = "Kazusa";
        SetPortraitImage(selectedCharacter);
        characterName.text = "ī���";
    }

    public void OnMariClicked()
    {
        selectedCharacter = "CH0186";
        SetPortraitImage(selectedCharacter);
        characterName.text = "����";
    }

    public void OnNikoClicked()
    {
        Debug.Log("Niko");
        selectedCharacter = "CH0172";
        SetPortraitImage(selectedCharacter);
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

    public void OnClickSelectButton()
    {
        //selectedCharacter = "CH0186";
        //SetPortraitImage(selectedCharacter);
        //characterName.text = "����";
        //SceneManager.LoadScene("DevRoomScene");

        SceneController.Instance.LoadScene("DevRoomScene");
    }

    public void SetPortraitImage(string name)
    {
        string portraitPath = path + name;
        Texture2D characterPortrait = Resources.Load<Texture2D>(portraitPath);
        characterImage.texture = characterPortrait;
    }

}
