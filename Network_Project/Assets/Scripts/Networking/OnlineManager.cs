using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineManager : MonoBehaviour
{
    public static OnlineManager instance;

    public GameObject localPlayer;
    public GameObject networkPlayer;

    private UDP_Client _client;
    private UDP_Server _server;
    private Rigidbody2D networkRigidbody;

    public Transform serverSpawn;
    public Transform clientSpawn;

    public GameObject[] tower_Client = new GameObject[3];
    public GameObject[] tower_Host = new GameObject[3];

    public Tower initialTowerHost;
    public Tower initialTowerClient;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetUpTowers();

        networkRigidbody = networkPlayer.GetComponent<Rigidbody2D>();
        ReplicationManager.instance.networkRigidbody = networkRigidbody;
        ReplicationManager.instance.localPlayer = localPlayer;

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

    private void SetUpTowers()
    {
        for (int i = 0; i < tower_Client.Length; i++)
        {
            tower_Client[i].name = "TClient_" + i;
            if (NetworkData.ConnectionType == ConnectionType.Client)
            {
                tower_Client[i].GetComponent<Tower_Buying_Platform>().player = localPlayer.GetComponent<Player_Stats>();
            }
            else
            {
                tower_Client[i].GetComponent<Tower_Buying_Platform>().player = networkPlayer.GetComponent<Player_Stats>();
            }
        }

        for (int i = 0; i < tower_Host.Length; i++)
        {
            tower_Host[i].name = "THost_" + i;
            if (NetworkData.ConnectionType == ConnectionType.Server)
            {
                tower_Host[i].GetComponent<Tower_Buying_Platform>().player = localPlayer.GetComponent<Player_Stats>();
            }
            else
            {
                tower_Host[i].GetComponent<Tower_Buying_Platform>().player = networkPlayer.GetComponent<Player_Stats>();
            }
        }

        if (NetworkData.ConnectionType == ConnectionType.Client)
        {
            initialTowerClient.GetComponent<Tower>().player_stats = localPlayer.GetComponent<Player_Stats>();
        }
        else
        {
            initialTowerClient.GetComponent<Tower>().player_stats = networkPlayer.GetComponent<Player_Stats>();
        }

        if (NetworkData.ConnectionType == ConnectionType.Server)
        {
            initialTowerHost.GetComponent<Tower>().player_stats = localPlayer.GetComponent<Player_Stats>();
        }
        else
        {
            initialTowerHost.GetComponent<Tower>().player_stats = networkPlayer.GetComponent<Player_Stats>();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }


    private void OnPacketRecieved(NetworkPacket packet)
    {
        //Debug.Log("Packet Recieved");
        if (packet.type == PacketType.Hello)
        {
            _client.Connected = true;
        }
        ReplicationManager.instance.ProcessPacket(packet);
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
            while (_client.Connected == false)
            {
                HelloPacket packet = new HelloPacket();
                byte[] data = packet.ToByteArray();
                _client.SendPacket(data);

                yield return new WaitForSeconds(0.01f);
            }


        }



    }
}
