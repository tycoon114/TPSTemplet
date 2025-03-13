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
    }

    void OnDisable()
    {
        GunController.onAmmoChanged -= UpdateAmmoUI; // 이벤트 해제
        GunController.CrossHairSet -= CrossHairSet;

    }

    void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }

    void UpdateHPUI(int currentHP, int maxHP)
    {

    }

    public void CrossHairSet(bool isAim) {

        if (isAim)
        {
            crossHair.SetActive(true);
        }
        else { 
            crossHair.SetActive(false);
        }
    }

    //플레이어 초상화
    void SetPlayerPortrait(string studentName)
    {
        Debug.Log(studentName);
    }

    //플레이어의 무기
    void SetPlayerWeaponIcon()
    {

    }


}
