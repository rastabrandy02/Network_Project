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
    private Socket _socket;
    private EndPoint _endPoint; //This endpoint is where you want to connect aka server IPAddress
    private int _port = 6969;
    private Thread _userThread;

    public string IP { get; set; }
    public string userName { get; set; }

    public GameObject joinServerUI;
    public GameObject lobbyUI;

    private string _savedServerName;
    private object _clientMutex = new object(); //used to lock variables so one one thread can use them at one time

    private bool _connected = false;

    private void Start()
    {
        lobbyUI.SetActive(false);
        joinServerUI.SetActive(true);
    }

    private void Update()
    {
        if (_socket != null)
        {
            if (_connected && !lobbyUI.activeSelf)
            {
                lobbyUI.SetActive(true);
                joinServerUI.SetActive(false);
                lobbyUI.GetComponent<LobbyUI>().SetNames(userName, _savedServerName);
            }
        }
    }

    public void ConnectToServer()
    {
        if (IP == "")
        {
            Debug.LogError("IP is null RAAAAAAAH");
            return;
        }

        bool isValid = IPAddress.TryParse(IP, out var iPAddress);

        if (!isValid)
        {
            Debug.LogError("IP isn't valid nigga");
            return;
        }

        _endPoint = new IPEndPoint(iPAddress, _port);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            _socket.Connect(_endPoint);
            Debug.Log("Trying to connect to server...");

            _userThread = new Thread(ServerCommunicate);
            _userThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server gyatt rizz ohio ////" + e);
            return;
        }
    }

    void ServerCommunicate()
    {
       //Registering Client
        {
            //Send User Name
            byte[] data = Encoding.ASCII.GetBytes(userName); //converting username to the data which will be sent to the server
            _socket.Send(data, data.Length, SocketFlags.None); //send data to server
            Debug.Log("Sent user name to server");
            
            //Recieve Server Name
            data = new byte[2048];
            int recievedBytes = _socket.Receive(data);

            string serverName = Encoding.ASCII.GetString(data, 0, recievedBytes);
            Debug.Log("Server Name is: " + serverName);

            lock (_clientMutex)
            {
                _savedServerName = serverName;
                _connected = true;
            }           
        }
        
        //Chat
        while (true)
        {
            byte[] data = new byte[2048];
            int recievedBytes = _socket.Receive(data);

            string chatMessage = Encoding.ASCII.GetString(data, 0, recievedBytes);
            Debug.Log(chatMessage);
        }
    }

    public void SendMessage()
    {
        ChatMessage message = new ChatMessage("Sexoooo", userName, DateTime.Now.ToString("dd/MM hh:mm"));
        string jsonMessage = JsonUtility.ToJson(message);

        _socket.Send(Encoding.ASCII.GetBytes(jsonMessage));
    }


    private void OnDestroy()
    {
        //Destroy Thread
        if (_userThread != null)
        {
            if (_userThread.IsAlive)
            {
                _userThread.Abort();
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

