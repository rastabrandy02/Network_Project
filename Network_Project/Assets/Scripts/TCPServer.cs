using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TCPServer : MonoBehaviour
{
    private Thread _acceptThread;
    private EndPoint _endPoint;
    private Socket _socket;
    private int _port = 6969;

    private object _clientMutex = new object(); //used to lock variables so one one thread can use them at one time
    private List<NetworkSocket> _connectedClients = new List<NetworkSocket>();
    private List<Thread> _clientThreads = new List<Thread>();

    public TMPro.TextMeshProUGUI IPText;
    public TMPro.TextMeshProUGUI clientsText;

    private bool _refreshList = true;

    private void Start()
    {
        InitServer();
        SetServerIP_UI();
    }

    private void Update()
    {
        if (_refreshList) RefreshList();
    }

    void InitServer()
    {
        //Create & bind endpoint and socket
        _endPoint = new IPEndPoint(IPAddress.Any, _port);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Bind(_endPoint);

        Debug.Log("Created server, listenig on port " + _port);
        _socket.Listen(10);

        _acceptThread = new Thread(AcceptConnections);
        _acceptThread.Start();

    }

    //print the Server IP on the canvas of the create server scene
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
            Socket client = _socket.Accept();
            Debug.Log("A client has connected! \nIP: " + client.RemoteEndPoint + ", Port: " + _port);

            NetworkSocket netSocket = new NetworkSocket("Default", client);

            Thread clientThread = new Thread(() => RecieveMessage(netSocket));

            lock (_clientMutex) //locking so other threads don't access the variables
            {
                _connectedClients.Add(netSocket);
                _clientThreads.Add(clientThread);
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

           
            _refreshList = true;
            

            //Send Server Name
            data = Encoding.ASCII.GetBytes("John Server Inventor de los Servers");
            netSocket.socket.Send(data);
        }
    }

    void RefreshList()
    {
        clientsText.text = "";

        foreach (var client in _connectedClients)
        {
            clientsText.text += client.userName + "\n";
        }

        _refreshList = false;
    }

    private void OnDestroy()
    {
        //Destroy Threads
        if (_acceptThread != null)
        {
            if (_acceptThread.IsAlive)
            {
                _acceptThread.Abort();

            }
        }

        foreach (var thread in _clientThreads)
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







