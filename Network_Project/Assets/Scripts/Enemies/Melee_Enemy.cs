using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Melee_Enemy : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float minDistanceToTarget;
    [SerializeField] float attackSpeed;
    [SerializeField] float damage;
    [SerializeField] int enemyValue;
    [SerializeField]Healthbar healthbar;

    Player_Stats targetPlayer;
    Enemy_Manager enemyManager;

    Transform[] pathPoints;
    Transform pathTarget;

    float health;
    bool isAlive = true;

    int pathPointIndex = 0;  
    float distanceTraveled;
    Vector3 lastPos;

    float timeSinceLastHit = 0.0f;
   
    Rigidbody2D rb;

    delegate void State();
    State state;

    void Start()
    {
        
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        state = FollowPath;      
        pathTarget = pathPoints[pathPointIndex];
        lastPos = transform.position;

        StartCoroutine(CheckDistance());
        StartCoroutine(UpdateTraveledDistance());
    }

    
    void FixedUpdate()
    {
        healthbar.SetHealth(health, maxHealth);
        state();
        if(health <= 0)
        {
            state = Die;
        }
    }

    void FollowPath()
    {
        if(rb.velocity.magnitude < maxSpeed)
        {
            Vector2 direction = (pathTarget.position - transform.position).normalized;
            rb.velocity += direction * acceleration;
        }
    }
    void Die()
    {
        isAlive = false;
        enemyManager.EnemyDead(enemyValue, targetPlayer);
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
    IEnumerator UpdateTraveledDistance()
    {
        while(isAlive) 
        {
            distanceTraveled += Vector2.Distance(transform.position, lastPos);
            lastPos = transform.position;
            yield return new WaitForSeconds(0.1f);
        
        }
    }

    public void SetPath(Transform[] path)
    {
        pathPoints = path;
    }
    public void SetTargetPlayer(Player_Stats targetPlayer)
    {
        this.targetPlayer = targetPlayer;
    }
    public void SetManager(Enemy_Manager manager)
    {
        enemyManager = manager;
    }
    public void TakeDamage(float damage)
    {
        health-=damage;
    }
    public float GetHealth()
    {
        return health; 
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public float GetDistanceTraveled()
    { 
        return distanceTraveled; 
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Base"))
        {
            timeSinceLastHit += Time.deltaTime;
            if(timeSinceLastHit >= attackSpeed)
            {
                collision.gameObject.GetComponent<Player_Base>().TakeDamage(damage);
                timeSinceLastHit = 0;
            }
        }
    }
}

