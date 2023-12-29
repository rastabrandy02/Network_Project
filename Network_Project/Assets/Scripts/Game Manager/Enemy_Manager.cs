using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    [SerializeField] Player_Stats player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDead(int enemyValue, Player_Stats _player)
    {
       if (player == _player)
        {
            _player.GiveCoins(enemyValue);

        }
    }
}
