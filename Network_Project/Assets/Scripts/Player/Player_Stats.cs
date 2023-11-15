using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
    int coins;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetCoins()
    {
        return coins;
    }
    public void GiveCoins(int coins)
    {
        this.coins += coins;       
    }
    public void SpendCoins(int coins)
    {
        this.coins -= coins;
    }
}
