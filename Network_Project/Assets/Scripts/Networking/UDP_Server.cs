using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class UDP_Server : MonoBehaviour
{
    int port = 9999;
    Thread acceptThread;
    Socket _socket;
    
    private List<NetworkSocket> connectedClients = new List<NetworkSocket>();
    private List<Thread> clientThreads = new List<Thread>();

    public TextMeshProUGUI IPText;
    public TextMeshProUGUI clientsText;

    private object clientMutex = new object();
    bool refreshUserList = true;
    void Start()
    {
        InitServer();
        SetServerIP_UI();
    }
    void Update()
    {
        if (refreshUserList) RefreshList();
    }

    void InitServer()
    {

        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);

        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        _socket.Bind(ipep);

        Debug.Log("Created server, listenig on port " + port);

        acceptThread = new Thread(AcceptConnections);
        acceptThread.Start();
    }
    void SetServerIP_UI()
    {
        IPAddress hostIP = IPAddress.Any;

        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                hostIP = ip;
            }
        }

        IPText.text = "IP: " + hostIP.ToString();
    }
    
    void AcceptConnections()
    {
        while (true)
        {
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            byte[] data = new byte[2048];

            int recievedBytes = _socket.ReceiveFrom(data, ref endPoint);

            Debug.Log("New client! -  IP: " + endPoint);

            string userName = Encoding.ASCII.GetString(data, 0, recievedBytes);

            NetworkSocket netSocket = new NetworkSocket(userName, null, endPoint);

            Thread clientThread = new Thread(RecieveMessage);

            lock (clientMutex)
            {
                connectedClients.Add(netSocket);
                clientThreads.Add(clientThread);
            }

            clientThread.Start();

            refreshUserList = true;


            //Send Server Name
            data = Encoding.ASCII.GetBytes("UDP Serverino");
            _socket.SendTo(data, endPoint);
        }
    }
    void RecieveMessage()
    {
        while (true)
        {
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

            //Recieve User Name
            byte[] data = new byte[2048];
            int recievedBytes = _socket.ReceiveFrom(data, ref endPoint);

            string userName = Encoding.ASCII.GetString(data, 0, recievedBytes);
            Debug.Log("User Name is: " + userName);

            refreshUserList = true;


            //Send Server Name
            data = Encoding.ASCII.GetBytes("UDP Serverino");
            _socket.SendTo(data, endPoint);
        }
    }

    void RefreshList()
    {
        clientsText.text = "";

        foreach (var client in connectedClients)
        {
            clientsText.text += client.userName + "\n";
        }

        refreshUserList = false;
    }

    private void OnDestroy()
    {
        //Destroy Threads
        if (acceptThread != null)
        {
            if (acceptThread.IsAlive)
            {
                acceptThread.Abort();

            }
        }

        foreach (var thread in clientThreads)
        {
            if (thread.IsAlive)
            {
                thread.Abort();
            }
        }

        //Destroy Socket
        if (_socket != null)
        {
            if (_socket.Connected)
            {
                _socket.Shutdown(SocketShutdown.Both);
            }

            _socket.Close();
        }
    }




}

