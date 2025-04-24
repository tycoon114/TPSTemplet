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
    public string selectedCharacter = "temp";       //제이슨 값을 가져오기 위해 선택한 캐릭터 명을 담을 변수
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
        //confirmButton.interactable = true;  // 선택하면 확정 버튼 활성화
    }

    public void OnKazusaClicked()
    {
        selectedCharacter = "Kazusa";
        SetPortraitImage(selectedCharacter);
        selectedVoice = selectedCharacter + selectedVoice;
        SoundManager.Instance.PlaySelectVoiceSfx(selectedVoice);
        characterName.text = "카즈사";
    }

    public void OnMariClicked()
    {
        selectedCharacter = "CH0186";
        SetPortraitImage(selectedCharacter);
        selectedVoice = selectedCharacter + selectedVoice;
        SoundManager.Instance.PlaySelectVoiceSfx(selectedVoice);
        characterName.text = "마리";
    }

    public void OnNikoClicked()
    {
        Debug.Log("Niko");
        selectedCharacter = "CH0172";
        SetPortraitImage(selectedCharacter);
        characterName.text = "니코(음성X)";
    }

    public void OnShirokoClicked()
    {
        Debug.Log("Shiroko");
        selectedCharacter = "Shiroko";
        SetPortraitImage(selectedCharacter);
        selectedVoice = selectedCharacter + selectedVoice;
        SoundManager.Instance.PlaySelectVoiceSfx(selectedVoice);
        characterName.text = "시로코";
    }

    public void OnWakamoClicked()
    {
        selectedCharacter = "Wakamo";
        SetPortraitImage(selectedCharacter);
        selectedVoice = selectedCharacter + selectedVoice;
        SoundManager.Instance.PlaySelectVoiceSfx(selectedVoice);
        characterName.text = "와카모";
    }


    public void OnCharacterConfirm()
    {

    }

    public void OnClickSelectButton()
    {
        Debug.Log("선택Scene UI 코드상 인덱스  " + selectedCharacterIndex);

        SelectedCharacterIndex?.Invoke(selectedCharacterIndex);

        var bytes = System.Text.Encoding.ASCII.GetBytes(selectedCharacterIndex.ToString());
        NetworkManager.Singleton.NetworkConfig.ConnectionData = bytes;

        // Host 또는 Client 구분해서 시작
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
