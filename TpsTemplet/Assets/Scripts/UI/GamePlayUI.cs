using TMPro;
using UnityEngine;

public class GamePlayUI : MonoBehaviour
{

    public TextMeshProUGUI ammoText;
    void OnEnable()
    {
        GunController.onAmmoChanged += UpdateAmmoUI; // 이벤트 구독
    }

    void OnDisable()
    {
        GunController.onAmmoChanged -= UpdateAmmoUI; // 이벤트 해제
    }

    void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }

}
