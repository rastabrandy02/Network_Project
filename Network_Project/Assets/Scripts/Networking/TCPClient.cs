using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TCPClient : MonoBehaviour
{
    private EndPoint _endPoint;
    private NetSocket _socket;
    private int _port = 6969;
    public TMPro.TMP_InputField inputField;

    public TMPro.TMP_InputField userNameField;

    public void ConnectToServer()
    {
        if (!IPAddress.TryParse(inputField.text, out IPAddress address))
        {
            Debug.LogError("Could not parse IP");
            return;
        }

        _endPoint = new IPEndPoint(address, _port);
        _socket = new NetSocket(userNameField.text, ConnectionType.Client, StreamType.TCP, (IPEndPoint)_endPoint);
        ((TcpClient)_socket.socket).Client.Connect(_endPoint);

        NetworkData.NetSocket = _socket;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}

