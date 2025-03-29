using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public static SceneController Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SoundManager.Instance.PlayBGM("Theme_22");
    }

    public void LoadScene(string sceneName)
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySfx("buttonTouch");
        SceneManager.LoadScene(sceneName);
        //해당 메뉴에 맞는 BGM 재생
        if (sceneName == "MenuScene")
        {
            SoundManager.Instance.PlayBGM("Theme_22");
        }
        else if (sceneName == "CharacterSelectScene")
        {
            SoundManager.Instance.PlayBGM("Theme_16");
        }
        Debug.Log(sceneName + "  불러옴");
    }

    public void ExitScene()
    {
        Application.Quit();
    }
}
