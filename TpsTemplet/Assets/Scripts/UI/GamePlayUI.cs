using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI hpText;
    public Image portraitImage;
    public Image weaponImage;

    public GameObject crossHair;

    private string selectedCharacterName;
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            selectedCharacterName = player.transform.GetChild(0).name; // 캐릭터의 이름 가져오기
            SetPlayerPortrait(selectedCharacterName);
        }
    }


    void OnEnable()
    {
        GunController.onAmmoChanged += UpdateAmmoUI; // 이벤트 구독
        GunController.CrossHairSet += CrossHairSet;
        PlayerManager.UpdateHPUI += UpdateHPUI;
    }

    void OnDisable()
    {
        GunController.onAmmoChanged -= UpdateAmmoUI; // 이벤트 해제
        GunController.CrossHairSet -= CrossHairSet;
        PlayerManager.UpdateHPUI -= UpdateHPUI;
    }

    void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }

    void UpdateHPUI(int currentHP, int maxHP)
    {
        hpText.text = $"{currentHP} / {maxHP}";
    }

    public void CrossHairSet(bool isAim)
    {
        if (isAim)
        {
            crossHair.SetActive(true);
        }
        else
        {
            crossHair.SetActive(false);
        }
    }

    //플레이어 초상화
    void SetPlayerPortrait(string studentName)
    {
        string portraitPath = "Image/portrait/Texture2D/Student_Portrait_" + studentName;
        //string portraitPath = "Image/portraitSkillsize/Texture2D/Skill_Portrait_" + studentName;
        string weaponPortraitPath = "Image/weapon/Texture2D/Weapon_Icon_" + studentName;
        Debug.Log(studentName);
        Sprite characterPortrait = Resources.Load<Sprite>(portraitPath);
        Sprite weaponPortrait = Resources.Load<Sprite>(weaponPortraitPath);

        if (characterPortrait != null)
        {
            portraitImage.sprite = characterPortrait;
        }
        else
        {
            Debug.LogWarning("초상화 이미지가 없습니다: " + studentName);
            Debug.Log(portraitPath);
        }

        if (weaponPortraitPath != null)
        {
            weaponImage.sprite = weaponPortrait;
        }
        else
        {
            Debug.LogWarning("무기 이미지가 없습니다: " + studentName);
        }
    }
}
