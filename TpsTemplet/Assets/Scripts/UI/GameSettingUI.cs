using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingUI : MonoBehaviour
{
    public GameObject settingObj;

    public TextMeshProUGUI resolutiionText;
    public TextMeshProUGUI graphicText;
    public TextMeshProUGUI fullScreenText;

    //각 토글 마다 해상도를 설정하고, 누른 것을 불러오는 방식으로
    //배열에담고 값이  바뀌는 것을 확인 하는 방식

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

    //해상도 변경
    public void OnToggleResolutionChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            //Debug.Log(toggle.name);
            resolution = toggle.name;   
        }
    }

    //그래픽 변경
    public void OnToggleGraphicChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            //Debug.Log(toggle.name);
        }
    }

    //풀스크린
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
        //string[] res = resolutions[resolutionIndex].Split('X');   //각 토글 버튼 마다 해상도를 정해두고 , 이를 alpha X Beta 값으로 설정
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
