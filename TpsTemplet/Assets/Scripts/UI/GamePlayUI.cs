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
            selectedCharacterName = Player.name; // 캐릭터의 이름 가져오기
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
        GunController.onAmmoChanged += UpdateAmmoUI; //탄약 수
        GunController.CrossHairSet += CrossHairSet;     //조준 시 크로스헤어 활성화
        PlayerManager.UpdateHPUI += UpdateHPUI;         //플레이어 체력
        PlayerController.SetSkillUI += SetSkillUI;     //스킬 아이콘 알파값
    }

    void OnDisable()
    {
        PlayerController.OnLocalPlayerSpawned -= InitUIWithPlayer;
        GunController.onAmmoChanged -= UpdateAmmoUI; // 이벤트 해제
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
        Debug.Log("탄약 테스트");
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

    //플레이어 초상화
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
            Debug.LogWarning("초상화 이미지가 없습니다: " + cleanedName);
            Debug.Log(portraitPath);
        }

        if (weaponPortraitPath != null)
        {
            weaponImage.sprite = weaponPortrait;
        }
        else
        {
            Debug.LogWarning("무기 이미지가 없습니다: " + cleanedName);
        }
    }

    void SetSkillUI(bool isSkillUsed)
    {
        Debug.Log("스킬 테스트");
        StartCoroutine(UpdateSkillUIAlpha());  
    }

    private IEnumerator UpdateSkillUIAlpha()
    {
        //임시 스킬 쿨 타임
        float tempSkillCool = 10.0f;

        //기본 알파값
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
