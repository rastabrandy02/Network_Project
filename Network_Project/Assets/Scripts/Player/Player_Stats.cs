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
        if (Input.GetKeyDown(KeyCode.Space)) GiveCoins(10);
    }
    public int GetCoins()
    {
        return coins;
    }
    public void GiveCoins(int coins)
    {
        this.coins += coins;       
    }
    public bool SpendCoins(int coins)
    {
        if(this.coins - coins < 0) return false;
        this.coins -= coins;
        return true;
    }
}
