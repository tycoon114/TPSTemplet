using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingUI : MonoBehaviour
{
    public GameObject settingObj;

    public TextMeshProUGUI resolutiionText;
    public TextMeshProUGUI graphicText;
    public TextMeshProUGUI fullScreenText;

    //�� ��� ���� �ػ󵵸� �����ϰ�, ���� ���� �ҷ����� �������
    //�迭����� ����  �ٲ�� ���� Ȯ�� �ϴ� ���

    private int resolutionIndex = 0;
    private int qualityIndex = 0;
    private bool isFullScreen = true;

    private string resolution = "fHd";


    [Header("Resolution")]
    public Toggle fullHDToggle;
    public Toggle qHDToggle;

    [Header("Graphic")]
    public Toggle exHighToggle;
    public Toggle highToggle;
    public Toggle middleToggle;
    public Toggle lowToggle;

    [Header("Fullscreen Mode")]
    public Toggle fullscreenToggle;
    public Toggle windowedToggle;

    [Header("Sound")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {

    }

    public void OnSettingExitClick()
    {
        SoundManager.Instance.PlaySfx("buttonTouch");
        settingObj.SetActive(false);
    }

    //�ػ� ����
    public void OnToggleResolutionChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            //Debug.Log(toggle.name);
            resolution = toggle.name;   
        }
    }

    //�׷��� ����
    public void OnToggleGraphicChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            //Debug.Log(toggle.name);
        }
    }

    //Ǯ��ũ��
    public void OnToggleFullScreenChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            if (toggle.name.Equals("graphicToggleFullScreen"))
            {
                isFullScreen = true;
                //Debug.Log(isFullScreen);
            }
            else
            {
                isFullScreen = false;
                //Debug.Log(isFullScreen);
            }
        }
    }

    public void OnApplySettingClick()
    {
        SoundManager.Instance.PlaySfx("buttonTouch");
        ApplySetting();
        SaveSetting();
    }

    private void ApplySetting()
    {
        SoundManager.Instance.PlaySfx("buttonTouch");
        //string[] res = resolutions[resolutionIndex].Split('X');   //�� ��� ��ư ���� �ػ󵵸� ���صΰ� , �̸� alpha X Beta ������ ����
        //Debug.Log(resolution);
        //Debug.Log(isFullScreen);
    }

    private void SaveSetting()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.SetInt("GraphicQualityIndex", qualityIndex);
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSetting()
    {

    }
    public void OnBgmVolumeChanged()
    {
        float value = bgmSlider.value;
        Debug.Log(value);
        SoundManager.Instance.SetBGMVolume(value);
    }

    public void OnSfxVolumeChanged()
    {
        float value = sfxSlider.value;
        Debug.Log(value);
        SoundManager.Instance.SetSfxVolume(value);
    }

}
