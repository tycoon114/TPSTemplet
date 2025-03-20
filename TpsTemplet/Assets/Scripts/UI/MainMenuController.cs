using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public Button testRoom;

    void Awake()
    {
        
    }

    public void OnDevRoomClick() {
        Debug.Log("Å¬¸¯ÇÔ");
        SceneController.Instance.LoadScene("DevRoomScene");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
