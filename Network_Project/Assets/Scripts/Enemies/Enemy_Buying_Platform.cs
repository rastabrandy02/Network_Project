using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Buying_Platform : MonoBehaviour
{
    [SerializeField] int enemyCost;
    [SerializeField] Enemy_Spawner targetSpawner;
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
            if (Input.GetKey(KeyCode.E))
            {              
                if(collision.gameObject.GetComponent<Player_Stats>().SpendCoins(enemyCost))
                {
                    targetSpawner.SpawnSiegeEnemy();                   
                }
                
            }
        }
    }
    
}
