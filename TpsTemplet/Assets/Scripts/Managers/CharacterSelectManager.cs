using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{

    private TextMeshProUGUI characterName;
    private Image characterImage;
    private string selectedCharacter;       //제이슨 값을 가져오기 위해 선택한 캐릭터 명을 담을 변수

    private void Awake()
    {
        characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        characterImage = GameObject.Find("CharacterImage").GetComponent<Image>();
    }

    public void OnKazusaClicked()
    {
        Debug.Log("Kazusa");
        selectedCharacter = "Kazusa";
        characterName.text = "카즈사";
    }

    public void OnMariClicked()
    {
        Debug.Log("Mari");
        selectedCharacter = "Mari";
        characterName.text = "마리";
    }

    public void OnNikoClicked()
    {
        Debug.Log("Niko");
        selectedCharacter = "Niko";
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
}
