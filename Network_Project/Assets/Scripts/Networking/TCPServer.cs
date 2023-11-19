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
    private EndPoint _endPoint;
    private NetSocket _socket;
    private int _port = 6969;

    private object _clientMutex = new object(); //used to lock variables so one one thread can use them at one time
    private List<NetworkSocket> _connectedClients = new List<NetworkSocket>();

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
        _socket = new NetSocket("Mongolia", ConnectionType.Server, StreamType.TCP, (IPEndPoint)_endPoint);


        Debug.Log("Created server, listenig on port " + _port);
        TcpListener listener = (TcpListener)_socket.socket;
        listener.Start();
        listener.BeginAcceptTcpClient(new AsyncCallback(AcceptConnections), new StateObject(_socket));

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

    void AcceptConnections(IAsyncResult AR)
    {
        StateObject stateObject = (StateObject)AR.AsyncState;
        NetSocket netSocket = stateObject.socket;
        TcpListener listener = (TcpListener)netSocket.socket;

        //accept connection
        TcpClient client = listener.EndAcceptTcpClient(AR);
        Debug.Log("A client has connected! \nIP: " + client.Client.RemoteEndPoint + ", Port: " + _port);

        //Create empty socket and fill with client data
        NetSocket clientNetSocket = new NetSocket();
        clientNetSocket.socket = client;
        clientNetSocket.streamType = StreamType.TCP;
        clientNetSocket.connectionType = ConnectionType.Client;

        //listen for connected client data
        StateObject clientState = new StateObject(clientNetSocket);
        listener.Server.BeginReceive(clientState.buffer, 0, 2048, 0, new AsyncCallback (RecieveMessage), clientState);
        
        //continue listening for future client connections
        listener.BeginAcceptTcpClient(new AsyncCallback(AcceptConnections), stateObject);

    }

    void RecieveMessage(IAsyncResult AR)
    {
        StateObject stateObject = (StateObject)AR.AsyncState;
        NetSocket netSocket = stateObject.socket;
        TcpListener listener = (TcpListener)netSocket.socket;
        //Registering Client
        {
            //Recieve User Name
            int recievedBytes = listener.Server.EndReceive(AR);

            string userName = Encoding.ASCII.GetString(stateObject.buffer, 0, recievedBytes);
            Debug.Log("User Name is: " + userName);

            netSocket.userName = userName;


            _refreshList = true;
        }

        //Recieving Chat Messages
        //while (true)
        //{
        //    byte[] data = new byte[2048];
        //   int recievedBytes = netSocket.socket.Client.Receive(data);

        //    if (recievedBytes == 0)
        //    {
        //        lock (_clientMutex)
        //        {
        //            _connectedClients.Remove(netSocket);
        //            Debug.Log("Client " + netSocket.userName + " has disconnected. Adiós aweonao");
        //            break;
        //        }
        //    }

        //    foreach (var client in _connectedClients)
        //    {
        //        client.socket.Send(data);
        //    }

        //    string chatMessage = Encoding.ASCII.GetString(data, 0, recievedBytes);
        //    Debug.Log(chatMessage);
        //}
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
        
    }
}







