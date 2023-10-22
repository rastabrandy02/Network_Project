using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class UDP_Client : MonoBehaviour
{
    public string IP { get; set; }
    public string userName { get; set; }

    string serverName;
    int port = 9999;

    Thread communicateThread;
    Socket mySocket;
    object clientMutex = new object();

    bool isConnected = false;

    public GameObject joinServerUI;
    public GameObject lobbyUI;

    void Start()
    {
        lobbyUI.SetActive(false);
        joinServerUI.SetActive(true);
    }
    void Update()
    {
        if (mySocket != null)
        {
            if (isConnected && !lobbyUI.activeSelf)
            {
                lobbyUI.SetActive(true);
                joinServerUI.SetActive(false);
                lobbyUI.GetComponent<LobbyUI>().SetNames(userName, serverName);
            }
        }
    }

    public void ConnectToServer()
    {
        if (IP == "")
        {
            Debug.LogError("IP is null");
            return;
        }

        bool isValid = IPAddress.TryParse(IP, out var iPAddress);

        if (!isValid)
        {
            Debug.LogError("invalid IP");
            return;
        }

        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(IP), port);

        mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        try
        {
            mySocket.Connect(ipep);
            Debug.Log("Trying to connect to server...");

            communicateThread = new Thread(CommunicateWithServer);
            communicateThread.IsBackground = true;
            communicateThread.Start();

        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server " + e);
            return;
        }

    }
    void CommunicateWithServer()
    {
        while (true)
        {
            //Send User Name
            byte[] data = Encoding.ASCII.GetBytes(userName);
            mySocket.Send(data, data.Length, SocketFlags.None);
            Debug.Log("Sent user name to server");

            //Recieve Server Name
            data = new byte[2048];
            int recievedBytes = mySocket.Receive(data);

            string savedServerName = Encoding.ASCII.GetString(data, 0, recievedBytes);
            Debug.Log("Server Name is: " + savedServerName);


            lock (clientMutex)
            {
                savedServerName = serverName;
                isConnected = true;
            }
            break;
        }
    }
    private void OnDestroy()
    {
        //Destroy Thread
        if (communicateThread != null)
        {
            if (communicateThread.IsAlive)
            {
                communicateThread.Abort();
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
