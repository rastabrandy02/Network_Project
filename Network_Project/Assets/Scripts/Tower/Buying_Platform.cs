using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buying_Platform : MonoBehaviour
{
    [SerializeField] int towerCost;
    [SerializeField] GameObject tower;
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
            if(Input.GetKeyDown(KeyCode.E))
            {
                collision.gameObject.GetComponent<Player_Stats>().SpendCoins(towerCost);
                Instantiate(tower, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
