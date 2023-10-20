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
    public string myIp = "127.0.0.1";  
    public int port = 0;
    string storedText;

    private static UDPReceive instance;
  

    private void Awake()
    {
        // check if another instance already exists
        if (instance && instance != this)
        {
            Debug.LogError("There is already another instance of UDPReceive in the scene! Only one is allowed!", this);
            Debug.LogError("That's true, I am the active instance for this scene", instance);
            
            Destroy(this.gameObject);
            return;
        }       
        instance = this;

        DontDestroyOnLoad(this.gameObject);

        storedText = "Awaiting to receive data...";
    }
    public void Start()
    {
        Init();
    }

    private void Init()
    {                
        
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
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(myIp), port);
        udpClient.Connect(remoteEndPoint);
        while (true)
        {

            try
            {                             
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                Debug.Log(data.Length);
                //udpClient.ReceiveAsync();
                string text = Encoding.UTF8.GetString(data);              
                Debug.Log(text + " "); 
                storedText = text;
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
    public string GetStoredData()
    {
        return storedText;
    }

}
