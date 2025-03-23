using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{

    private TextMeshProUGUI characterName;
    private Image characterImage;

    private void Awake()
    {
        characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        characterImage = GameObject.Find("CharacterImage").GetComponent<Image>();
    }

    public void OnKazusaClicked()
    {
        Debug.Log("Kazusa");
        characterName.text = "카즈사";
    }

    public void OnMariClicked()
    {
        Debug.Log("Mari");
        characterName.text = "마리";
    }

    public void OnNikoClicked()
    {
        Debug.Log("Niko");
        characterName.text = "니코";
    }

    public void OnShirokoClicked()
    {
        Debug.Log("Shiroko");
        characterName.text = "시로코";
    }
}
