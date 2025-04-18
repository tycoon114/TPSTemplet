using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI hpText;
    public Image portraitImage;
    public Image weaponImage;
    public Image skillImage;
    //public Image ultImage;

    public GameObject crossHair;
    public GameObject escMenu;
    public GameObject settingsObj;

    public Transform Player;

    public bool isEscMenuActive = false;
    public bool isSettingActive = false;

    private string selectedCharacterName;

    private float originalAlpha;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        escMenu.SetActive(false);
        settingsObj.SetActive(false);

        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var p in players)
        {
            var netObj = p.GetComponent<NetworkObject>();
            if (netObj != null && netObj.IsOwner)
            {
                Player = p.transform;
                break;
            }
        }


        if (Player != null)
        {
            selectedCharacterName = Player.name; // ĳ������ �̸� ��������
            SetPlayerPortrait(selectedCharacterName);
        }
        originalAlpha = skillImage.color.a;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            isEscMenuActive = !isEscMenuActive;
            escMenu.SetActive(isEscMenuActive);
        }

        if (isEscMenuActive == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnRetrunGameClick()
    {
        SoundManager.Instance.PlaySfx("buttonTouch");
        isEscMenuActive = false;
        escMenu.SetActive(false);
    }

    public void OnSettingClick()
    {
        SoundManager.Instance.PlaySfx("buttonTouch");
        isSettingActive = !isSettingActive;
        settingsObj.SetActive(isSettingActive);
    }

    public void OnReturnMainMenuClick()
    {
        //if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
        //{
        //    NetworkManager.Singleton.Shutdown();
        //}
        NetworkManager.Singleton.Shutdown();
        SceneController.Instance.LoadScene("MenuScene");
    }

    void OnEnable()
    {
        PlayerController.OnLocalPlayerSpawned += InitUIWithPlayer;
        GunController.onAmmoChanged += UpdateAmmoUI; //ź�� ��
        GunController.CrossHairSet += CrossHairSet;     //���� �� ũ�ν���� Ȱ��ȭ
        PlayerManager.UpdateHPUI += UpdateHPUI;         //�÷��̾� ü��
        PlayerController.SetSkillUI += SetSkillUI;     //��ų ������ ���İ�
    }

    void OnDisable()
    {
        PlayerController.OnLocalPlayerSpawned -= InitUIWithPlayer;
        GunController.onAmmoChanged -= UpdateAmmoUI; // �̺�Ʈ ����
        GunController.CrossHairSet -= CrossHairSet;
        PlayerManager.UpdateHPUI -= UpdateHPUI;
        PlayerController.SetSkillUI -= SetSkillUI;
    }

    private void InitUIWithPlayer(Transform playerTransform)
    {
        Player = playerTransform;
        selectedCharacterName = Player.name;
        SetPlayerPortrait(selectedCharacterName);
    }


    void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        Debug.Log("ź�� �׽�Ʈ");
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }
    void UpdateHPUI(float currentHP, float maxHP)
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
        string cleanedName = studentName.Replace("(Clone)", "");


        string portraitPath = "Image/portrait/Texture2D/Student_Portrait_" + cleanedName;
        string weaponPortraitPath = "Image/weapon/Texture2D/Weapon_Icon_" + cleanedName;
        //stringplayerSkillIconPath = "Image/portraitSkillsize/Texture2D/Skill_Portrait_" + studentName;


        Debug.Log(cleanedName);
        Sprite characterPortrait = Resources.Load<Sprite>(portraitPath);
        Sprite weaponPortrait = Resources.Load<Sprite>(weaponPortraitPath);
        Sprite playerSkillIcon;

        if (characterPortrait != null)
        {
            portraitImage.sprite = characterPortrait;
        }
        else
        {
            Debug.LogWarning("�ʻ�ȭ �̹����� �����ϴ�: " + cleanedName);
            Debug.Log(portraitPath);
        }

        if (weaponPortraitPath != null)
        {
            weaponImage.sprite = weaponPortrait;
        }
        else
        {
            Debug.LogWarning("���� �̹����� �����ϴ�: " + cleanedName);
        }
    }

    void SetSkillUI(bool isSkillUsed)
    {
        Debug.Log("��ų �׽�Ʈ");
        StartCoroutine(UpdateSkillUIAlpha());  
    }

    private IEnumerator UpdateSkillUIAlpha()
    {
        //�ӽ� ��ų �� Ÿ��
        float tempSkillCool = 10.0f;

        //�⺻ ���İ�
        Color skillImageColor = skillImage.color;

        skillImageColor.a = 0.3f;
        skillImage.color = skillImageColor;

        float elapsed = 0f;
        while (elapsed < tempSkillCool)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0.3f, originalAlpha, elapsed / tempSkillCool);
            skillImageColor.a = alpha;
            yield return null;
        }
        skillImageColor.a = originalAlpha;
        skillImage.color = skillImageColor;
    }
}
