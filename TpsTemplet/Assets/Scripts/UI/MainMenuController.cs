using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public static bool isHost = false;
    public Button testRoom;

    void Awake()
    {

    }

    public void OnDevRoomClick()
    {
        SceneController.Instance.LoadScene("CharacterSelectScene");
    }

    public void OnDevRoomClient()
    {
        isHost = false ;
        SceneController.Instance.LoadScene("CharacterSelectScene");
    }


    public void OnDevRoomHost()
    {
        isHost = true;
        SceneManager.LoadScene("CharacterSelectScene");
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
