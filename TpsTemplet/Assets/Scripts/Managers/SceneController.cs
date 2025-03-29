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
        //�ش� �޴��� �´� BGM ���
        if (sceneName == "MenuScene")
        {
            SoundManager.Instance.PlayBGM("Theme_22");
        }
        else if (sceneName == "CharacterSelectScene")
        {
            SoundManager.Instance.PlayBGM("Theme_16");
        }
        Debug.Log(sceneName + "  �ҷ���");
    }

    public void ExitScene()
    {
        Application.Quit();
    }
}
