using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public void JoinGame()
    {
        SceneManager.LoadScene("Join Game Scene");
        NetworkData.ConnectionType = ConnectionType.Client;
    }

    public void CreateGame()
    {
        SceneManager.LoadScene("Lobby");
        NetworkData.ConnectionType = ConnectionType.Server;
    }
}
