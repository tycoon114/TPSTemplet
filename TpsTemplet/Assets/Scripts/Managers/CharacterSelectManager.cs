using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CharacterSelectManager : MonoBehaviour
{
    private TextMeshProUGUI characterName;
    private RawImage characterImage;
    public string selectedCharacter = "temp";       //제이슨 값을 가져오기 위해 선택한 캐릭터 명을 담을 변수
    private string path = "Image/portrait/Texture2D/Student_Portrait_";

    public int selectedCharacterIndex;

    //개발 테스트를 용이하게 하기 위해 싱글톤 사용, 멀티 플레이로 작업할 때는 유니티 넷코드 사용할 것
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
        //confirmButton.interactable = true;  // 선택하면 확정 버튼 활성화
    }

    public void OnKazusaClicked()
    {
        selectedCharacter = "Kazusa";
        SetPortraitImage(selectedCharacter);
        characterName.text = "카즈사";
    }

    public void OnMariClicked()
    {
        selectedCharacter = "CH0186";
        SetPortraitImage(selectedCharacter);
        characterName.text = "마리";
    }

    public void OnNikoClicked()
    {
        Debug.Log("Niko");
        selectedCharacter = "CH0172";
        SetPortraitImage(selectedCharacter);
        characterName.text = "니코";
    }

    public void OnShirokoClicked()
    {
        Debug.Log("Shiroko");
        selectedCharacter = "Shiroko";
        characterName.text = "시로코";
    }


    public void OnCharacterConfirm()
    {

    }

    public void OnClickSelectButton()
    {
        //selectedCharacter = "CH0186";
        //SetPortraitImage(selectedCharacter);
        //characterName.text = "마리";
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
