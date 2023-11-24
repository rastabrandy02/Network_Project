using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public enum ConnectionType
{
    None,
    Server,
    Client
}

public enum StreamType
{
    None,
    TCP,
    UDP
}

public class StateObject
{
    public NetSocket socket;
    public byte[] buffer = new byte[2048];
       public StateObject(NetSocket _socket) 
    {
        socket = _socket;
    }
}
public class NetSocket
{
    public object socket;
    public string userName;
    public ConnectionType connectionType;
    public StreamType streamType;

    public NetSocket() {}
    public NetSocket(string name, ConnectionType conType, StreamType _streamType, IPEndPoint endPoint)
    {
        userName = name;
        connectionType = conType;
        streamType = _streamType;

        if (connectionType == ConnectionType.Client)
        {
            if (streamType == StreamType.UDP)
            {
                socket = new UdpClient();
            }
            else
            {
                socket = new TcpClient();
            }
        }
        else
        {
            if (streamType == StreamType.UDP)
            {
               //pal futuro aweonao pinfloi
            }
            else
            {
                socket = new TcpListener(endPoint);
            }
        }
    }
}

public static class NetworkData
{
    public const int Port = 9999;
    public static IPAddress ServerAddress;
    public static NetSocket NetSocket;
    public static List<NetSocket> ConnectedClients;
    public static ConnectionType ConnectionType;
}