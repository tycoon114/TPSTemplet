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
    }

    public void LoadScene(string sceneName)
    {
        //이후 캐릭터 선택에서 소리를 나눌수도 있음
        SoundManager.Instance.PlaySfx("buttonTouch");
        SceneManager.LoadScene(sceneName);
        //해당 메뉴에 맞는 BGM 재생
        if (sceneName == "MenuScene")
        {
            SoundManager.Instance.PlayBGM("bgmTemp1");
        }
        Debug.Log(sceneName + "  불러옴");
    }

    public void ExitScene()
    {
        Application.Quit();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
