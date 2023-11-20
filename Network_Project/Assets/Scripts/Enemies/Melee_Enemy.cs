using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Melee_Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float minDistanceToTarget;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float damage;
    [SerializeField] protected int enemyValue;    

    protected Player_Stats targetPlayer;
    protected Enemy_Manager enemyManager;

    protected Transform[] pathPoints;
    protected Transform pathTarget;

    protected float health;
    protected bool isAlive = true;

    protected int pathPointIndex = 0;
    protected float distanceTraveled;
    protected Vector3 lastPos;

    protected float timeSinceLastHit = 0.0f;

    protected Rigidbody2D rb;
    protected Healthbar healthbar;

    protected delegate void State();
    protected State state;

    void Start()
    {               
        rb = GetComponent<Rigidbody2D>();
        healthbar = GetComponentInChildren<Healthbar>();

        health = maxHealth;
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            timeSinceLastHit += Time.deltaTime;
            if (timeSinceLastHit >= attackSpeed)
            {
                collision.gameObject.GetComponent<Player_Base>().TakeDamage(damage);
                timeSinceLastHit = 0;
            }
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
    
   
}

