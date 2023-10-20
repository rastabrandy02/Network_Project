using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

public class UDPSend : MonoBehaviour
{
    private static int localPort;

   
    public string myIP = "127.0.0.1";
    public int port = 0;

    
    IPEndPoint remoteEndPoint;
    UdpClient udpClient;
    
    void Start()
    {
        Init();
    }

    
    void Update()
    {
        
    }

    public void Init()
    {       
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(myIP), port);
        udpClient = new UdpClient();
        udpClient.Connect(remoteEndPoint);
       
        Debug.Log("Sending to " + myIP + " : " + port);

    }
    public void SendData()
    {        
        SendString("This is a test message");
    }
    private void SendString(string message)
    {
        try
        {           
            byte[] data = Encoding.UTF8.GetBytes(message);
        
            udpClient.Send(data, data.Length);
            Debug.Log("Message sent");

            
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }
}
