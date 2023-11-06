using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Enemy : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float speed;
    [SerializeField] float minDistanceToTarget;


    Transform[] pathPoints;
    Transform pathTarget;

    int pathPointIndex = 0;
    float health;
    bool isAlive = true;


    delegate void State();
    State state;

    void Start()
    {
        health = maxHealth;

        state = FollowPath;      
        pathTarget = pathPoints[pathPointIndex];
        StartCoroutine(CheckDistance());
        
    }

    // Update is called once per frame
    void Update()
    {
        state();
        if(health <= 0)
        {
            state = Die;
        }
    }

    void FollowPath()
    {
        transform.position += new Vector3(pathTarget.position.x - transform.position.x, pathTarget.position.y - transform.position.y, 0.0f) * speed * Time.deltaTime;
    }
    void Die()
    {
        isAlive = false;
        Destroy(gameObject);
    }
    IEnumerator CheckDistance()
    {
        while(isAlive)
        {
            if (Vector2.Distance(transform.position, pathTarget.position) < minDistanceToTarget)
            {

                if (pathPointIndex < pathPoints.Length - 1) pathPointIndex++;

                pathTarget = pathPoints[pathPointIndex];
            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    public void SetPath(Transform[] path)
    {
        pathPoints = path;
    }
    public void TakeDamage(float damage)
    {
        health-=damage;
    }
}
