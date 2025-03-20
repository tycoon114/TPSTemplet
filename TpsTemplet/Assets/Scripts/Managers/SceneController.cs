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
        //���� ĳ���� ���ÿ��� �Ҹ��� �������� ����
        SoundManager.Instance.PlaySfx("buttonTouch");
        SceneManager.LoadScene(sceneName);
        //�ش� �޴��� �´� BGM ���
        if (sceneName == "MenuScene")
        {
            SoundManager.Instance.PlayBGM("bgmTemp1");
        }
        Debug.Log(sceneName + "  �ҷ���");
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
