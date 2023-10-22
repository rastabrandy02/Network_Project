using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public void JoinGame()
    {
        SceneManager.LoadScene("Join Game Scene");
    }

    public void CreateGame()
    {
        SceneManager.LoadScene("Create Game Scene");
    }
}
