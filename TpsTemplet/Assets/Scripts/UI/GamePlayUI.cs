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
            selectedCharacterName = player.transform.GetChild(0).name; // ĳ������ �̸� ��������
            SetPlayerPortrait(selectedCharacterName);
        }
    }


    void OnEnable()
    {
        GunController.onAmmoChanged += UpdateAmmoUI; // �̺�Ʈ ����
        GunController.CrossHairSet += CrossHairSet;
        PlayerManager.UpdateHPUI += UpdateHPUI;
    }

    void OnDisable()
    {
        GunController.onAmmoChanged -= UpdateAmmoUI; // �̺�Ʈ ����
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

    //�÷��̾� �ʻ�ȭ
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
            Debug.LogWarning("�ʻ�ȭ �̹����� �����ϴ�: " + studentName);
            Debug.Log(portraitPath);
        }

        if (weaponPortraitPath != null)
        {
            weaponImage.sprite = weaponPortrait;
        }
        else
        {
            Debug.LogWarning("���� �̹����� �����ϴ�: " + studentName);
        }
    }
}
