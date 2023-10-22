using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI userText;
    public TMPro.TextMeshProUGUI serverText;

    public void SetNames(string userName, string serverName)
    {
        userText.text = "User Name: \n" + userName;
        serverText.text = "Server Name: \n" + serverName;

    }
}
