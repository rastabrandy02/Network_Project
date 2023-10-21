using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class UDP_Client : MonoBehaviour
{
    public string uiText;
    Thread connectThread;
    Thread communicateThread;
    EndPoint Remote;
    Socket server;
    
   
    void Start()
    {
        connectThread = new Thread(new ThreadStart(ConnectThread));
        connectThread.IsBackground = true;
        connectThread.Start();
    }
   
    

    void ConnectThread()
    {
        byte[] data = new byte[1024];
       
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

        server = new Socket(AddressFamily.InterNetwork,
                       SocketType.Dgram, ProtocolType.Udp);
        
        string welcome = "Hello, are you there?";
        data = Encoding.ASCII.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        Remote = sender;

        communicateThread = new Thread(new ThreadStart(CommunicateWithServer));
        communicateThread.IsBackground = true;
        communicateThread.Start();
    }

     void CommunicateWithServer()
    {
        byte[] data = new byte[1024];
        int recv = server.ReceiveFrom(data, ref Remote);

        Debug.Log("Message received from {0}:" + Remote.ToString());
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));

        string message = "Hello, I'm dummyUser01";
        

        while (true)
        {

            server.SendTo(Encoding.ASCII.GetBytes(message), Remote);
            data = new byte[1024];
            recv = server.ReceiveFrom(data, ref Remote);
            uiText = Encoding.ASCII.GetString(data, 0, recv);
            Debug.Log(uiText);
        }
    }
}
