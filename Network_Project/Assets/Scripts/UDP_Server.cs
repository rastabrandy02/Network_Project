using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using System.Threading;

public class UDP_Server : MonoBehaviour
{
    public string uiText;
    Thread connectThread;
    Thread communicateThread;
    EndPoint Remote;
    Socket client;
    byte[] data;
    void Start()
    {
        connectThread = new Thread(new ThreadStart(ConnectThread));
        connectThread.IsBackground = true;
        connectThread.Start();


    }
    void ConnectThread()
    {

        data = new byte[1024];
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);

        client = new Socket(AddressFamily.InterNetwork,
                        SocketType.Dgram, ProtocolType.Udp);

        client.Bind(ipep);
        Debug.Log("Waiting for a client...");

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        Remote = sender;

        communicateThread = new Thread(new ThreadStart(CommunicateWithClient));
        communicateThread.IsBackground = true;
        communicateThread.Start();
    }

    void CommunicateWithClient()
    {
        int recv;
        recv = client.ReceiveFrom(data, ref Remote);

        Debug.Log("Message received from:" + Remote.ToString());
        uiText = Encoding.ASCII.GetString(data, 0, recv);
        Debug.Log(uiText);

        string welcome = "Welcome to the test server";

        while (true)
        {
            data = Encoding.ASCII.GetBytes(welcome);
            client.SendTo(data, data.Length, SocketFlags.None, Remote);

            data = new byte[1024];
            recv = client.ReceiveFrom(data, ref Remote);
            uiText = Encoding.ASCII.GetString(data, 0, recv);
            Debug.Log(uiText);
            //client.SendTo(data, recv, SocketFlags.None, Remote);
        }
    }



}

