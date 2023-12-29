using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Buying_Platform : MonoBehaviour
{
    [SerializeField] int towerCost;
    [SerializeField] GameObject tower;

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
        if(collision.gameObject.CompareTag("Player"))
        {
            if(Input.GetKey(KeyCode.E) && canSpawn)
            {
                if(collision.gameObject.GetComponent<Player_Stats>().SpendCoins(towerCost))
                {
                    SpawnPacket packet = new SpawnPacket();
                    packet.tower_id = int.Parse(name.Split('_')[1]);
                    OnlineManager.instance.SendPacket(packet);
                    Spawn();
                }
                
            }
        }
    }
   
    public void Spawn()
    {
        if(canSpawn == false)
        {
            return;
        }

        canSpawn = false;
        Instantiate(tower, transform.position, Quaternion.identity);
       

        Destroy(gameObject);
    }
}
