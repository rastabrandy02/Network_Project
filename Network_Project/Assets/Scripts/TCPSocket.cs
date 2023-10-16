using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TCPSocket : MonoBehaviour
{
    
    
    public void Init()
    {
        MenuManager.textTestServer = "TCP Server";

        socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ip = new IPEndPoint(IPAddress.Any, serverPort);
        socketServer.Bind(ip);

        socketServer.Listen(1);

        Receiving();
    }

    void Update()
    {
        if (closeServer)
        {
            socketClient.Close();
            socketServer.Close();

            thread.Abort();
        }
    }
}





