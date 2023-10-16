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

   
    private string myIP;  // define in init
    public int port;  // define in init

    
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

        myIP = "127.0.0.1";
        port = 8051;

        
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(myIP), port);
        udpClient = new UdpClient();

       
        Debug.Log("Sending to " + myIP + " : " + port);

    }
    public void SendData()
    {
        
        sendString("This is a test message");
    }
    private void sendString(string message)
    {
        try
        {
            
            byte[] data = Encoding.UTF8.GetBytes(message);

            
            udpClient.Send(data, data.Length, remoteEndPoint);
            
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }
}
