using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Security.Claims;
using Unity.Netcode;
using System.Runtime.CompilerServices;


public class CharacterSelectUI : MonoBehaviour
{

    public static event Action<int> SelectedCharacterIndex;

    private TextMeshProUGUI characterName;
    private RawImage characterImage;
    public string selectedCharacter = "temp";       //���̽� ���� �������� ���� ������ ĳ���� ���� ���� ����
    private string path = "Image/portrait/Texture2D/Student_Portrait_";

    private int voiceNum;
    private string selectedVoice = "Select";

    public int selectedCharacterIndex;

    private void Awake()
    {
        characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        characterImage = GameObject.Find("CharacterImage").GetComponent<RawImage>();

        voiceNum = UnityEngine.Random.Range(1, 3);
        selectedVoice = selectedVoice + voiceNum;
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
        selectedVoice = selectedCharacter + selectedVoice;
        SoundManager.Instance.PlaySelectVoiceSfx(selectedVoice);
        characterName.text = "ī���";
    }

    public void OnMariClicked()
    {
        selectedCharacter = "CH0186";
        SetPortraitImage(selectedCharacter);
        selectedVoice = selectedCharacter + selectedVoice;
        SoundManager.Instance.PlaySelectVoiceSfx(selectedVoice);
        characterName.text = "����";
    }

    public void OnNikoClicked()
    {
        Debug.Log("Niko");
        selectedCharacter = "CH0172";
        SetPortraitImage(selectedCharacter);
        characterName.text = "����(����X)";
    }

    public void OnShirokoClicked()
    {
        Debug.Log("Shiroko");
        selectedCharacter = "Shiroko";
        SetPortraitImage(selectedCharacter);
        selectedVoice = selectedCharacter + selectedVoice;
        SoundManager.Instance.PlaySelectVoiceSfx(selectedVoice);
        characterName.text = "�÷���";
    }

    public void OnWakamoClicked()
    {
        selectedCharacter = "Wakamo";
        SetPortraitImage(selectedCharacter);
        selectedVoice = selectedCharacter + selectedVoice;
        SoundManager.Instance.PlaySelectVoiceSfx(selectedVoice);
        characterName.text = "��ī��";
    }


    public void OnCharacterConfirm()
    {

    }

    public void OnClickSelectButton()
    {
        Debug.Log("����Scene UI �ڵ�� �ε���  " + selectedCharacterIndex);

        SelectedCharacterIndex?.Invoke(selectedCharacterIndex);

        var bytes = System.Text.Encoding.ASCII.GetBytes(selectedCharacterIndex.ToString());
        NetworkManager.Singleton.NetworkConfig.ConnectionData = bytes;

        // Host �Ǵ� Client �����ؼ� ����
        if (MainMenuController.isHost)
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }

        SceneController.Instance.LoadScene("DevRoomScene");
    }

    public void SetPortraitImage(string name)
    {
        string portraitPath = path + name;
        Texture2D characterPortrait = Resources.Load<Texture2D>(portraitPath);
        characterImage.texture = characterPortrait;
    }

}
