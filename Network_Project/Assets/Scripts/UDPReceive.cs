using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;


public class UDPReceive : MonoBehaviour
{
    
    Thread receiveThread;    
    UdpClient udpClient;
    // public string IP = "127.0.0.1"; default 
    public int port;

    public Text_Receiver_UI uiReceiver;


    private static UDPReceive instance;
  

    private void Awake()
    {
        // check if another instance already exists
        if (instance && instance != this)
        {
            Debug.LogError("There is already another instance of UDPReceive in the scene! Only one is allowed!", this);
            Debug.LogError("That's true, I am the active instance for this scene", instance);

            this.enabled = false;
            return;
        }
        
        instance = this;
        

    }
    public void Start()
    {

        Init();
    }

    private void Init()
    {                
        port = 8051;       
        Debug.Log("Receiving from 127.0.0.1 : " + port);
        
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;   
        receiveThread.Start();
    }

    // receive thread
    private void ReceiveData()
    {

        udpClient = new UdpClient(port);
        
        while (true)
        {

            try
            {               
                IPEndPoint myIP = new IPEndPoint(IPAddress.Any, port);
                byte[] data = udpClient.Receive(ref myIP);              
                string text = Encoding.UTF8.GetString(data);              
                Debug.Log(text + " "); 
                uiReceiver.ChangeText(text);
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
            
        }
        
    }

    void OnApplicationQuit()
    {

        if (receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
        udpClient.Close();
    }
}
