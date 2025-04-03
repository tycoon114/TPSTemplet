using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class CharacterSelectManager : MonoBehaviour
{
    public string selectedCharacter = "temp";

    public int selectedCharacterIndex;

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
