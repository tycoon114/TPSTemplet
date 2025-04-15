using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Security.Claims;
using Unity.Netcode;


public class CharacterSelectUI : MonoBehaviour
{

    public static event Action<int> SelectedCharacterIndex;

    private TextMeshProUGUI characterName;
    private RawImage characterImage;
    public string selectedCharacter = "temp";       //���̽� ���� �������� ���� ������ ĳ���� ���� ���� ����
    private string path = "Image/portrait/Texture2D/Student_Portrait_";

    public int selectedCharacterIndex;

    private void Awake()
    {
        characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        characterImage = GameObject.Find("CharacterImage").GetComponent<RawImage>();

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
        SetPortraitImage(selectedCharacter);
        characterName.text = "�÷���";
    }

    public void OnWakamoClicked()
    {
        selectedCharacter = "Wakamo";
        SetPortraitImage(selectedCharacter);
        characterName.text = "��ī��";
    }


    public void OnCharacterConfirm()
    {

    }

    public void OnClickSelectButton()
    {
        int selectedIndex = selectedCharacterIndex;
        string payload = selectedIndex.ToString();
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(payload);

        NetworkManager.Singleton.StartHost();
        SelectedCharacterIndex?.Invoke(selectedCharacterIndex);
        SceneController.Instance.LoadScene("DevRoomScene");
    }

    public void SetPortraitImage(string name)
    {
        string portraitPath = path + name;
        Texture2D characterPortrait = Resources.Load<Texture2D>(portraitPath);
        characterImage.texture = characterPortrait;
    }

}
