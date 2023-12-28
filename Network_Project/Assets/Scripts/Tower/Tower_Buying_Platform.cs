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
                    canSpawn = false;
                    Instantiate(tower, transform.position, Quaternion.identity);
                    SpawnPacket packet = new SpawnPacket();
                    //FindObjectOfType<OnlineManager>().SendPacket(packet);
                  
                    Destroy(gameObject);
                }
                
            }
        }
    }
   
}
