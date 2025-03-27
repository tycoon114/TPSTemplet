using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CharacterSelectManager : MonoBehaviour
{
    public string selectedCharacter = "temp";

    public int selectedCharacterIndex;

    //개발 테스트를 용이하게 하기 위해 싱글톤 사용, 멀티 플레이로 작업할 때는 유니티 넷코드 사용할 것
    public static CharacterSelectManager Instance
    {
        get;
        private set;
    }


    private void OnEnable()
    {
        CharacterSelectUI.SelectedCharacterIndex += SetSelectCharacter;
    }

    private void OnDisable()
    {
        CharacterSelectUI.SelectedCharacterIndex -= SetSelectCharacter;
    }


    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SetSelectCharacter(int index)
    {
        selectedCharacterIndex = index;
    }
}
