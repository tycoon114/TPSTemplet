using TMPro;
using UnityEngine;

public class GamePlayUI : MonoBehaviour
{

    public TextMeshProUGUI ammoText;
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

}
