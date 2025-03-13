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
    }

    void OnDisable()
    {
        GunController.onAmmoChanged -= UpdateAmmoUI; // �̺�Ʈ ����
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

    //�÷��̾� �ʻ�ȭ
    void SetPlayerPortrait(string studentName)
    {
        Debug.Log(studentName);
    }

    //�÷��̾��� ����
    void SetPlayerWeaponIcon()
    {

    }


}
