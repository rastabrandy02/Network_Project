using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDP_Server : MonoBehaviour
{
    private EndPoint _endPoint;
    private Socket _socket;
    private int _port = 6969;
    private byte[] buffer;
    public Action<NetworkPacket> OnPacketRecieved;

    private List<NetworkPacket> PacketQueue = new();
    private object packetMutex = new object();

    private object mutex = new object();
    private bool clientConnected = false;

    private void Start()
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
        //Create & bind endpoint and socket
        _endPoint = new IPEndPoint(IPAddress.Any, 0);
        EndPoint endPoint = new IPEndPoint(IPAddress.Any, _port);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        _socket.Bind(endPoint);

        buffer = new byte[NetworkPacket.MAX_SIZE];
        _socket.BeginReceiveFrom(buffer, 0, NetworkPacket.MAX_SIZE, 0, ref _endPoint, new AsyncCallback(RecieveCallback), null);
    }


    void RecieveCallback(IAsyncResult AR)
    {
        int rBytes = _socket.EndReceiveFrom(AR, ref _endPoint);

        if (rBytes == 0) return; //Client has disconnected from server

        NetworkPacket packet = NetworkPacket.ParsePacket(buffer);
        Debug.Log(packet);
        lock (packetMutex)
        {
            PacketQueue.Add(packet);
        }

        lock (mutex)
        {
            clientConnected = true;
        }
        _socket.BeginReceiveFrom(buffer, 0, NetworkPacket.MAX_SIZE, 0, ref _endPoint, new AsyncCallback(RecieveCallback), null);

    }

    public void SendPacket(byte[] data)
    {        
        if (clientConnected)
            _socket.SendTo(data, 0, NetworkPacket.MAX_SIZE, SocketFlags.None, _endPoint);
    }

    private void OnDestroy()
    {

    }
}



