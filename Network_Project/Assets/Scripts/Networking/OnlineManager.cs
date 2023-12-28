using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineManager : MonoBehaviour
{
    public GameObject localPlayer;
    public GameObject networkPlayer;
    
    private UDP_Client _client;
    private UDP_Server _server;
    private Rigidbody2D networkRigidbody; 

    public Transform serverSpawn;
    public Transform clientSpawn;

    // Start is called before the first frame update
    void Start()
    {
        networkRigidbody = networkPlayer.GetComponent<Rigidbody2D>();

        if (NetworkData.ConnectionType == ConnectionType.Client)
        {
            var client = gameObject.AddComponent<UDP_Client>();
            client.OnPacketRecieved += OnPacketRecieved;
            _client = client;
            localPlayer.transform.position = clientSpawn.position;
            networkPlayer.transform.position = serverSpawn.position;
        }
        else
        {
            var server = gameObject.AddComponent<UDP_Server>();
            server.OnPacketRecieved += OnPacketRecieved;
            _server = server;
            networkPlayer.transform.position = clientSpawn.position;
            localPlayer.transform.position = serverSpawn.position;
        }
        
        StartCoroutine(SendPackets());
    }


    // Update is called once per frame
    void Update()
    {
       
    }   

    private void ProcessPlayerPos(PlayerPositionPacket packet)
    {
        networkRigidbody.MovePosition(new Vector2(packet.x, packet.y));
        //Debug.Log("X: " + packet.x + "Y: " + packet.y);
    }

    private void OnPacketRecieved(NetworkPacket packet)
    {
        switch (packet.type)
        {
            case PacketType.PlayerPosition:
                {
                    //Debug.Log("Holiwis");
                    ProcessPlayerPos((PlayerPositionPacket)packet);
                }
                break;
            case PacketType.Spawn:
                {
                    Debug.Log("Spawn Packet vaya pinga tiene Pablo");
                }
                break;
        }
    }

    public void SendPacket(NetworkPacket packet)
    {
        byte[] data = packet.ToByteArray();

        if (NetworkData.ConnectionType == ConnectionType.Client)
        {
            _client.SendPacket(data);
        }
        if (NetworkData.ConnectionType == ConnectionType.Server)
        {
            _server.SendPacket(data);
        }
    }

    private IEnumerator SendPackets()
    {
        if (NetworkData.ConnectionType == ConnectionType.Client)
        {
            HelloPacket packet = new HelloPacket();
            byte[] data = packet.ToByteArray();            
            _client.SendPacket(data);            
        }

        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            var position = localPlayer.transform.position;
            PlayerPositionPacket packet = new PlayerPositionPacket(position.x, position.y, 0);

            SendPacket(packet);
        }
           
    }
}
