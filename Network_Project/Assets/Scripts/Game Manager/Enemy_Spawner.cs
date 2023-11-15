using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    
    [SerializeField] GameObject meleeEnemy;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] bool canSpawn;
    [SerializeField] Transform[] pathPoints;
    [SerializeField] Player_Stats targetPlayer;

    int waveLevel = 1;

    float nextSpawn;
    int currentWave;
    int numberOfEnemiesToSpawn;

    Enemy_Manager enemyManager;

    void Start()
    {
        numberOfEnemiesToSpawn = waveLevel;
        enemyManager = GetComponent<Enemy_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawn) return;
        if (Time.time > nextSpawn)
        {
            SpawnEnemies(meleeEnemy, numberOfEnemiesToSpawn);
            currentWave++;
            nextSpawn = Time.time + timeBetweenSpawns;
            if (currentWave % 3 == 1)
            {
                IncreaseWaveDifficulty();
            }
            
        }
    }

    void SpawnEnemies(GameObject enemy, int n)
    {
        for(int i = 0; i< n; i++) 
        {
            GameObject go = Instantiate(enemy, transform.position, Quaternion.identity);
            Melee_Enemy meleeEnemy = go.GetComponent<Melee_Enemy>();
            meleeEnemy.SetPath(pathPoints);
            meleeEnemy.SetTargetPlayer(targetPlayer);
            meleeEnemy.SetManager(enemyManager);


        }


    }
    void IncreaseWaveDifficulty()
    {
        waveLevel++;
    }
}
