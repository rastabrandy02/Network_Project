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
    Socket mySocket;

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

        mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        mySocket.Bind(ipep);
        
        Debug.Log("Created server, listenig on port " + port);

        mySocket.Listen(10);

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
            Socket client = mySocket.Accept();

            Debug.Log("New client! -  IP: " + client.RemoteEndPoint + " Port: " + port);

            NetworkSocket netSocket = new NetworkSocket("Default", client);

            Thread clientThread = new Thread(() => RecieveMessage(netSocket));

            lock (clientMutex) 
            {
                connectedClients.Add(netSocket);
                clientThreads.Add(clientThread);
            }

            clientThread.Start();
        }
    }
    void RecieveMessage(NetworkSocket netSocket)
    {
        while (true)
        {
            //Recieve User Name
            byte[] data = new byte[2048];
            int recievedBytes = netSocket.socket.Receive(data);

            string userName = Encoding.ASCII.GetString(data, 0, recievedBytes);
            Debug.Log("User Name is: " + userName);

            netSocket.userName = userName;


            refreshUserList = true;


            //Send Server Name
            data = Encoding.ASCII.GetBytes("UDP Serverino");
            netSocket.socket.Send(data);
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
        if (mySocket != null)
        {
            if (mySocket.Connected)
            {
                mySocket.Shutdown(SocketShutdown.Both);
            }

            mySocket.Close();
        }
    }




}

