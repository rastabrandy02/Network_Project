using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Buying_Platform : MonoBehaviour
{
    [SerializeField] int towerCost;
    [SerializeField] GameObject tower;

    public Player_Stats player;

    bool canSpawn = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E) && canSpawn)
            {
                var _player_stats = collision.gameObject.GetComponent<Player_Stats>();
                if (_player_stats == player)
                {
                    if (_player_stats.SpendCoins(towerCost))
                    {
                        SpawnTowerPacket packet = new SpawnTowerPacket();
                        packet.tower_id = int.Parse(name.Split('_')[1]);
                        OnlineManager.instance.SendPacket(packet);
                        Spawn();
                    }
                }
            }
        }
    }

    public void Spawn()
    {
        if (canSpawn == false)
        {
            return;
        }

        canSpawn = false;
        GameObject go = Instantiate(tower, transform.position, Quaternion.identity);
        go.GetComponent<Tower>().player_stats = player;



        Destroy(gameObject);
    }
}
