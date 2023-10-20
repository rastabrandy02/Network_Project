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
    bool exit = false;
    bool closeServer = false;

    Socket socketServer;

    Socket socketClient;

    IPEndPoint ip;

    int serverPort = 5666;

    public string message = "Un video maaa mi gente";


    Thread thread;

    void Start()
    {

    }
    
    public void Init()
    {
       // MenuManager.textTestServer = "TCP Server";

        socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ip = new IPEndPoint(IPAddress.Any, serverPort);
        socketServer.Bind(ip);

        socketServer.Listen(1);

        Receiving();
    }

    void Update()
    {
        if (closeServer)
        {
            socketClient.Close();
            socketServer.Close();

            thread.Abort();
        }
    }

    void Sending()
    {
        if (socketClient != null)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            int byteSend = socketClient.Send(data);
            
            if(byteSend == message.Length)
            {
                Debug.Log("Server: Sent correctly..." + message);
               // MenuManger.consoleTestServer.Add("Server: Sent correctly..." + message);


                
            }
            else
            {
                Debug.Log("Server: Error not message sent");
                //MenuManger.consoleTestServer.Add("Server: Error not message sent");
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
        while(!exit)
        {
            if (socketClient == null)
            {
                socketClient = socketServer.Accept();

                Debug.Log("Server: Server client connected in the server" + ip.Address + "at port" + ip.Port);
                //MenuManager.consoleTestServer.Add("Server: Server client connected in the server" + ip.Adress + "at port" + ip.Port);

                byte[] data = new byte[68];

                try
                {
                    int receivedBytes = socketClient.Receive(data);
                    string msgRecieved = Encoding.ASCII.GetString(data);
                    string finalMsg = msgRecieved.Trim('\0');

                    if (receivedBytes > 0)
                    {
                        if (msgRecieved.Contains("Un video maaa mi gente"))
                        {
                            Debug.Log("Server: Received message correctly" + finalMsg);
                            //MenuManager.consoleTestServer.Add("Server: Recieved message correctly" + finalMsg);

                            Thread.Sleep(500);
                            Sending();
                        }
                        else if (msgRecieved.Contains("Destroy"))
                        {
                            Debug.Log("Server: Desconnection");
                            // MenuManager.consoleTestServer.Add("Server: Desconnection");
                            socketClient.Close();
                            socketClient = null;
                            socketServer.Close();
                            exit = true;
                            break;
                        }
                    }

                }

                catch
                {

                }



            }
            
        }
    }
    
    private void OnDestroy()
    {
        if (thread != null && thread.IsAlive && exit == false)
        {
            closeServer = true;
        }
    }
}





