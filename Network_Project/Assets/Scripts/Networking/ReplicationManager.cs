using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    public int netId = 0;

}


public class ReplicationManager : MonoBehaviour
{
    public static ReplicationManager instance;

    List<NetworkObject> netObjects = new();

    public GameObject localPlayer;
    public Rigidbody2D networkRigidbody;

    public Player_Base hostBase;
    public Player_Base clientBase;

    private void Awake()
    {

        instance = this;
    }
    void Start()
    {
        StartCoroutine(SendPlayerPacket());
    }

    void Update()
    {

    }

    private void ProcessPlayerPos(PlayerPositionPacket packet)
    {
        if (networkRigidbody.velocity == Vector2.zero)
        {
            networkRigidbody.MovePosition(new Vector2(packet.x, packet.y));
        }
        //Debug.Log("X: " + packet.x + "Y: " + packet.y);
    }

    public void ProcessPacket(NetworkPacket packet)
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
                    ProcessSpawn((SpawnPacket)packet);
                }
                break;
            case PacketType.BaseDamage:
                {
                    ProcessBaseDmg((BaseDmgPacket)packet);
                }
                break;
            case PacketType.PlayerMovement:
                {
                    ProcessPlayerMovement((PlayerMovementPacket)packet);
                }
                break;
        }
    }

    private void ProcessSpawn(SpawnPacket packet)
    {
        if (NetworkData.ConnectionType == ConnectionType.Client)
        {
            GameObject.Find("THost_" + packet.tower_id).GetComponent<Tower_Buying_Platform>().Spawn();
        }
        else
        {
            GameObject.Find("TClient_" + packet.tower_id).GetComponent<Tower_Buying_Platform>().Spawn();
        }
    }

    private void ProcessBaseDmg(BaseDmgPacket packet)
    {
        Debug.Log("PACKET PROCESSED // DAMAGE = " + packet.damage +  " // HOST " + packet.isHost);


        if (packet.isHost)
        {
            Debug.Log("HOST SHOULD BE TAKING DAMAGE MF");
            hostBase.TakeDamage(packet.damage);
        }
        else
        {
            clientBase.TakeDamage(packet.damage);
        }
    }

    private void ProcessPlayerMovement(PlayerMovementPacket packet)
    {
        networkRigidbody.velocity += packet.direction;
    }

        public void MakeNetObject(GameObject go)
    {
        NetworkObject no = go.AddComponent<NetworkObject>();
        netObjects.Add(no);
    }

    private IEnumerator SendPlayerPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            var position = localPlayer.transform.position;
            PlayerPositionPacket packet = new PlayerPositionPacket(position.x, position.y, 0);

            OnlineManager.instance.SendPacket(packet);

        }
    }
}

