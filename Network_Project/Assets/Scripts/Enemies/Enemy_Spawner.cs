using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{

    [SerializeField] GameObject meleeEnemy;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] Transform[] pathPoints;
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
            go.GetComponent<Melee_Enemy>().SetPath(pathPoints);


        }


    }
    void IncreaseWaveDifficulty()
    {
        waveLevel++;
    }
}
