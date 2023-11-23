using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TCPClientLobby : MonoBehaviour
{
    private NetSocket _socket = NetworkData.NetSocket;
    byte[] data = new byte[NetworkPacket.MAX_SIZE];
   
    void Start()
    {
        ((TcpClient)_socket.socket).Client.BeginReceive(data, 0, NetworkPacket.MAX_SIZE, 0, new AsyncCallback(OnRecieve), null);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnRecieve(IAsyncResult AR)
    {
        NetworkPacket packet = NetworkPacket.ParsePacket(data);
        if (packet.type == PacketType.StartGame)
        {
            SceneManager.LoadScene("Gameplay Scene");
        }
    }
}
