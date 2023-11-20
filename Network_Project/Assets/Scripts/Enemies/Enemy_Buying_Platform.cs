using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Buying_Platform : MonoBehaviour
{
    [SerializeField] int enemyCost;
    [SerializeField] Enemy_Spawner targetSpawner;
    [SerializeField] float platformCoolDown;

    Healthbar cooldownIndicator;

    bool canSpawn = true;
    float timeSinceLastSpawn;
    void Start()
    {
        cooldownIndicator = GetComponentInChildren<Healthbar>();
        timeSinceLastSpawn = platformCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canSpawn)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if(timeSinceLastSpawn >= platformCoolDown) 
            {             
                canSpawn = true;                
            }
        }
        cooldownIndicator.SetHealth(timeSinceLastSpawn, platformCoolDown);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {          
            if (Input.GetKey(KeyCode.E) && canSpawn)
            {              
                if(collision.gameObject.GetComponent<Player_Stats>().SpendCoins(enemyCost))
                {
                    targetSpawner.SpawnSiegeEnemy();
                    timeSinceLastSpawn = 0;
                    canSpawn = false;
                }
                
            }
        }
    }
    
}
