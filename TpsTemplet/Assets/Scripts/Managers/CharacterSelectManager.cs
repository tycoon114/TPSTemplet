using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{

    private TextMeshProUGUI characterName;
    private RawImage characterImage;
    private string selectedCharacter;       //제이슨 값을 가져오기 위해 선택한 캐릭터 명을 담을 변수
    private string path = "Image/portrait/Texture2D/Student_Portrait_";

    private void Awake()
    {
        characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        characterImage = GameObject.Find("CharacterImage").GetComponent<RawImage>();
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
    }

    public void SetPortraitImage(string name)
    {
        string portraitPath = path + name;
        Texture2D characterPortrait = Resources.Load<Texture2D>(portraitPath);
        characterImage.texture = characterPortrait;
    }

}
