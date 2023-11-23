using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject ServerMenu;
    public UnityEngine.UI.Button button;

    public TMPro.TextMeshProUGUI IPText;
    public TMPro.TextMeshProUGUI clientsText;
    void Start()
    {

        if (NetworkData.ConnectionType == ConnectionType.Server)
        {
            var server = gameObject.AddComponent<TCPServer>();
            button.onClick.AddListener(server.StartGame);
            server.IPText = IPText;
            server.clientsText = clientsText;
            ServerMenu.SetActive(true);
        }

        else
        {
            ServerMenu.SetActive(false);
            gameObject.AddComponent<TCPClientLobby>();
        }

    }

    void Update()
    {

    }
}
