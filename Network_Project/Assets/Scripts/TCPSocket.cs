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

    public NetworkSocket(string name, Socket sock)
    {
        userName = name;
        socket = sock;
    }
    
   
}





