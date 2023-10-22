using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class NetworkSocket
{
    public string userName;
    public Socket socket;
    public EndPoint endPoint;

    public NetworkSocket(string name, Socket sock, IPAddress Address, int port)
    {
        userName = name;
        socket = sock;
        endPoint = new IPEndPoint(Address, port);
    }

    public NetworkSocket(string name, Socket sock, EndPoint _endPoint)
    {
        userName = name;
        socket = sock;
        endPoint = _endPoint;
    }

}








