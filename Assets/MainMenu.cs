using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerTheReturn networkManger = null;

    //[Header("UI")]
    //[SerializeField] private GameObject landingPagePanel = null;

    public void HostLobby()
    {
        networkManger.StartHost();        
    }

    public void QuitGame()
    {
        Debug.Log("Game quit");
        Application.Quit();
    }
}
