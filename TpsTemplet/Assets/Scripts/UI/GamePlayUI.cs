using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{

    public TextMeshProUGUI ammoText;
    public Image portraitImage;

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
    }

    void OnDisable()
    {
        GunController.onAmmoChanged -= UpdateAmmoUI; // �̺�Ʈ ����
    }

    void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }

    void UpdateHPUI(int currentHP, int maxHP)
    {


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
