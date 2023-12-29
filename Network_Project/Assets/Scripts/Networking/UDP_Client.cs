using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDP_Client : MonoBehaviour
{
    private IPEndPoint _endPoint;
    private UdpClient _socket;

    private int _port = 6969;
    private byte[] buffer;

    public Action<NetworkPacket> OnPacketRecieved;
    public bool Connected = false;

    private List<NetworkPacket> PacketQueue = new();
    private object packetMutex = new object();

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

    private void Awake()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        //Create & bind endpoint and socket
        _port = NetworkData.Port + 1;
        _socket = new UdpClient(_port);
        _socket.Connect(NetworkData.ServerAddress, NetworkData.Port);
        buffer = new byte[NetworkPacket.MAX_SIZE];
        Debug.Log(_socket.Client.LocalEndPoint);
        _socket.BeginReceive(new AsyncCallback(RecieveCallback), null);
    }


    void RecieveCallback(IAsyncResult AR)
    {
        buffer = _socket.EndReceive(AR, ref _endPoint);
        Debug.Log("Gaychi");


        NetworkPacket packet = NetworkPacket.ParsePacket(buffer);

        lock (packetMutex)
        {
            PacketQueue.Add(packet);
        }

        _socket.BeginReceive(new AsyncCallback(RecieveCallback), null);
    }

    public void SendPacket(byte[] data)
    {
        _socket.Send(data, NetworkPacket.MAX_SIZE);
    }


    private void OnDestroy()
    {

    }
}

