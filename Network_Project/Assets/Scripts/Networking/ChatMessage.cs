using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessage
{
    public string message;
    public string user;
    public string time;

    public ChatMessage(string chatMessage, string chatUser, string chatTime)
    {
        message = chatMessage;
        user = chatUser;
        time = chatTime;
    }
   
}
