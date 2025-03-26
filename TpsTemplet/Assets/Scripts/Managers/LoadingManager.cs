using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

//�̱� ��带 ���� �� �δ�
//�¶��θ�忡�� ���� ����
//������ ���� �� �� �ִ�.
public class LoadingManager : MonoBehaviour
{

    public Slider loadingSlider;        //�ε� ��
    public Image loadingImage;          //�ε��� ������ �̹���

    private string nextScneName;


    public void StartLoading(string sceneName)
    {
        nextScneName = sceneName;
        StartCoroutine(LoadLoadingSceneAndNextScene());
    }



    IEnumerator LoadLoadingSceneAndNextScene()
    {
        //�ε� ���� �񵿱������� �ε�(�ε� ���� ǥ�ÿ��) , Additive�� ���� ���� ���� �����ϸ鼭 ���� ���� ���� �߰���
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        loadingScene.allowSceneActivation = false;

        //�ε����� �ε� �ɶ� ���� ���
        while (!loadingScene.isDone)
        {
            if (loadingScene.progress >= 0.9f)
            {
                loadingScene.allowSceneActivation = true;
            }
            yield return null;
        }
        FindLoadingSliderInScene();


        //���� ���� �񵿱������� �ε�
        AsyncOperation nextScene = SceneManager.LoadSceneAsync(nextScneName);
        while (!nextScene.isDone)
        {
            loadingSlider.value = nextScene.progress;
            yield return null;
        }
        SceneManager.UnloadSceneAsync("LoadingScene");
    }

    void FindLoadingSliderInScene()
    {
        loadingSlider = GameObject.Find("LoadingSlider").GetComponent<Slider>();
    }

}
