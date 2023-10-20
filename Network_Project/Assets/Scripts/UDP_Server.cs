using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDP_Server : MonoBehaviour
{
    Thread connectThread;
    Thread receiveThread;
    EndPoint Remote;
    Socket mySocket;
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

        mySocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Dgram, ProtocolType.Udp);

        mySocket.Bind(ipep);
        Console.WriteLine("Waiting for a client...");

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        Remote = sender;

        receiveThread = new Thread(new ThreadStart(ReceiveThread));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void ReceiveThread()
    {
        int recv;
        recv = mySocket.ReceiveFrom(data, ref Remote);

        Console.WriteLine("Message received from {0}:", Remote.ToString());
        Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));

        string welcome = "Welcome to my test server";
        data = Encoding.ASCII.GetBytes(welcome);
        mySocket.SendTo(data, data.Length, SocketFlags.None, Remote);
        while (true)
        {
            data = new byte[1024];
            recv = mySocket.ReceiveFrom(data, ref Remote);

            Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
            mySocket.SendTo(data, recv, SocketFlags.None, Remote);
        }
    }


   
}

   