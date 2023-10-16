using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Change_Scene_Button : MonoBehaviour
{
   public void ChangeScene()
    {
        if(SceneManager.GetActiveScene().name == "Create Game Scene")
        {          
            SceneManager.LoadSceneAsync("Join Game Scene");
        }
        if (SceneManager.GetActiveScene().name == "Join Game Scene")
        {           
            SceneManager.LoadSceneAsync("Create Game Scene");
        }
    }
}
