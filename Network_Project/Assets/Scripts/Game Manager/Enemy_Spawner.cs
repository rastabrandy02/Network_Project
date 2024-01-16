using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{

    [SerializeField] GameObject meleeEnemy;
    [SerializeField] GameObject siegeEnemy;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] bool canSpawn;
    [SerializeField] Transform[] pathPoints;
    [SerializeField] Player_Stats targetPlayer;
    [SerializeField] Enemy_Manager enemyManager;

    int waveLevel = 1;

    float nextSpawn;
    int currentWave;
    int numberOfEnemiesToSpawn;

    

    void Start()
    {
        numberOfEnemiesToSpawn = waveLevel;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawn) return;
        if (Time.time > nextSpawn)
        {
            SpawnMeleeEnemies(numberOfEnemiesToSpawn);
            currentWave++;
            nextSpawn = Time.time + timeBetweenSpawns;
            if (currentWave % 3 == 1)
            {
                IncreaseWaveDifficulty();
            }           
        }
    }

    void SpawnMeleeEnemies(int n)
    {
        for(int i = 0; i< n; i++) 
        {
            GameObject go = Instantiate(this.meleeEnemy, transform.position, Quaternion.identity);
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

    public void SpawnSiegeEnemy()
    {
        GameObject go = Instantiate(this.siegeEnemy, transform.position, Quaternion.identity);
        Melee_Enemy meleeEnemy = go.GetComponent<Melee_Enemy>();
        meleeEnemy.SetPath(pathPoints);
        meleeEnemy.SetTargetPlayer(targetPlayer);
        meleeEnemy.SetManager(enemyManager);

        SpawnEnemyPacket packet = new SpawnEnemyPacket();
        OnlineManager.instance.SendPacket(packet);
    }

    public void SpawnSiegeEnemyOpponent()
    {
        GameObject go = Instantiate(this.siegeEnemy, transform.position, Quaternion.identity);
        Melee_Enemy meleeEnemy = go.GetComponent<Melee_Enemy>();
        meleeEnemy.SetPath(pathPoints);
        meleeEnemy.SetTargetPlayer(targetPlayer);
        meleeEnemy.SetManager(enemyManager);
    }
}
