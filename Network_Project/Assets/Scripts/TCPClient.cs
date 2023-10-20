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
    bool exit = false;
    bool firstTimeSend = true;
    bool backUpSend = false;

    Socket socket;

    IPEndPoint ip;

    int serverPort = 5666;

    public string message = "Pa perdeer el tieeempo";

    Thread thread;

    int countPongs = 1;


    void Start()
    {
        
    }

    public void Init()
    {
        //MenuManager.textTestClient = "TCP Client";

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort);

        Receiving();
    }

    
    void Update()
    {
        if (firstTimeSend)
            Sending();
    }

    void Sending()
    {
        if (socket != null)
        {
            firstTimeSend = false;
            byte[] data = Encoding.ASCII.GetBytes(message); ;
            int bytesSend = socket.Send(data);

            if (bytesSend == message.Length)
            {
                Debug.Log("Client: Send Correctly " + message);
                //MenuManager.consoleTestClient.Add("Client: Send Correctly " + message);
            }
            else
            {
                Debug.Log("Client: Error sending");
                //MenuManager.consoleTestClient.Add("Client: Error sending");
            }
        }
    }

    void Receiving()
    {
        thread = new Thread(threadRecievesServerData);
        thread.Start();
    }

    void threadRecievesServerData()
    {
        socket.Connect(ip);

        Debug.Log("Client: Connected to the server " + ip.Address + " at port " + ip.Port);
        //MenuManager.consoleTestClient.Add("Client: Connected to the server " + ip.Address + " at port " + ip.Port);

        while (!exit)
        {
            byte[] data = new byte[68];

            try
            {
                int receivedBytes = socket.Receive(data);

                string msgRecieved = Encoding.ASCII.GetString(data);
                string finalMsg = msgRecieved.Trim('\0');
                if (receivedBytes > 0)
                {
                    if (msgRecieved.Contains("Un video maaa mi gente"))
                    {
                        Debug.Log("Client: Received Correctly: " + finalMsg);
                       // MenuManager.consoleTestClient.Add("Client: Received Correctly: " + finalMsg);

                        Thread.Sleep(500);
                        Sending();
                        countPongs++;
                        if (countPongs == 5)
                        {
                            message = "Destroy";
                            Sending();
                            exit = true;
                            break;
                        }
                    }
                }
            }
            catch
            {
                if (socket != null && backUpSend == false)
                {
                    Debug.Log("Client: Did not get message from server.");
                    //MenuManager.consoleTestClient.Add("Client: Did not get message from server.");
                    backUpSend = true;
                    message = "Pa perdeer el tieeempo";
                    Sending();
                   
                }
                else if (socket != null && backUpSend == true)
                {
                    socket.Close();
                    socket = null;
                    exit = true;

                }
            }
        }
    }

}