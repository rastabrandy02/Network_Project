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

public class NetSocket
{
    public UdpClient Socket;
    public string UserName;
    public ConnectionType ConnectionType;

    public NetSocket(string name, ConnectionType type)
    {
        UserName = name;
        ConnectionType = type;
    }
}

public static class NetworkData
{
    public const int Port = 9999;
    public static NetSocket NetSocket;       

}

public class NetServerSocket : NetSocket
{
    public List<NetSocket> ConnectedClients;
    public EndPoint ServerEndPoint;
    public NetServerSocket(string name)
        : base(name, ConnectionType.Server)
    {
    }
}