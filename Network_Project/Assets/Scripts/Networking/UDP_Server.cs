using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
public class PacketState
{
    public UdpClient Socket;
    public IPEndPoint EndPoint;
    public byte[] Data;
    ~PacketState()
    {
        Socket?.Dispose();
    }
}

public class UDP_Server : MonoBehaviour
{
    
    private int _port = 6969;
    private UdpClient _server;
    private IPEndPoint _endPoint;
    
    public Action<NetworkPacket> OnPacketRecieved;

    private List<NetworkPacket> PacketQueue = new();
    private object packetMutex = new object();

    private object mutex = new object();
    private bool clientConnected = false;

    private void Awake()
    {
        InitServer();
    }

    private void Update()
    {
        lock (packetMutex)
        {
            foreach (NetworkPacket p in PacketQueue)
            {
                OnPacketRecieved?.Invoke(p);
            }
            PacketQueue.Clear();
        }
    }

    void InitServer()
    {
        PacketState state = new PacketState();
        state.Data = new byte[NetworkPacket.MAX_SIZE];

        _server = new UdpClient(NetworkData.Port);
        state.Socket = _server;


        
        state.Socket.BeginReceive(new AsyncCallback(RecieveCallback), state);
    }


    void RecieveCallback(IAsyncResult AR)
    {
        PacketState state = (PacketState)AR.AsyncState;
        state.Data = state.Socket.EndReceive(AR, ref state.EndPoint);


        NetworkPacket packet = NetworkPacket.ParsePacket(state.Data);
        if (packet.type == PacketType.Hello)
        {

            
            lock (mutex)
            {
                _endPoint = state.EndPoint;
                clientConnected = true;
                SendPacket(state.Data);
            }
            
        }
        else
        {
            lock (packetMutex)
            {
                PacketQueue.Add(packet);
            }
        }
        

        
        state.Socket.BeginReceive(new AsyncCallback(RecieveCallback), state);
    }

    public void SendPacket(byte[] data)
    {
        if (clientConnected)
        {
            Debug.Log("Send poquet");
            
            _server.Send(data, NetworkPacket.MAX_SIZE, _endPoint);
        }
    }

    private void OnDestroy()
    {

    }
}



