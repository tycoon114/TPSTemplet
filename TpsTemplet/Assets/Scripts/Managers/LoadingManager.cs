using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

//싱글 모드를 위한 씬 로더
//온라인모드에선 쓰지 말것
//사용되지 않을 수 도 있다.
public class LoadingManager : MonoBehaviour
{

    public Slider loadingSlider;        //로딩 바
    public Image loadingImage;          //로딩시 보여줄 이미지

    private string nextScneName;


    public void StartLoading(string sceneName)
    {
        nextScneName = sceneName;
        StartCoroutine(LoadLoadingSceneAndNextScene());
    }



    IEnumerator LoadLoadingSceneAndNextScene()
    {
        //로딩 씬을 비동기적으로 로드(로드 상태 표시용씬) , Additive를 통해 기존 씬을 유지하면서 현재 씬에 씬을 추가함
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        loadingScene.allowSceneActivation = false;

        //로딩씬이 로드 될때 까지 대기
        while (!loadingScene.isDone)
        {
            if (loadingScene.progress >= 0.9f)
            {
                loadingScene.allowSceneActivation = true;
            }
            yield return null;
        }
        FindLoadingSliderInScene();


        //다음 씬을 비동기적으로 로드
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
