using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] float attackSpeed;
    [SerializeField] float damage;
    [SerializeField] float detectionRange;
    [SerializeField] float turretRotationSpeed;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform shootingPoint;
    [SerializeField] GameObject bullet;
    [SerializeField] LayerMask enemyLayerMask;

    float nextAttack;
    GameObject target;

    bool isDetecting;
   

    delegate void State();
    State state;
    void Start()
    {
        isDetecting = true;
       StartCoroutine(CheckTarget());
       
    }

    
    void Update()
    {
        
        state();
    }

    IEnumerator CheckTarget()
    {
        float biggestDistanceTravelled = 0f;

        while (isDetecting)
        {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayerMask);

            if (collisions != null)
            {               
                for (int i = 0; i < collisions.Length; i++)
                {
                    float currentDistanceTraveled = collisions[i].gameObject.GetComponent<Melee_Enemy>().GetDistanceTraveled();
                    if (currentDistanceTraveled > biggestDistanceTravelled)
                    {
                        target = collisions[i].gameObject;
                        biggestDistanceTravelled = currentDistanceTraveled;
                    }
                }

                if (target != null) 
                {
                    RotateTurret();
                    state = Attack;
                }
                else
                {
                    state = Idle;
                    biggestDistanceTravelled = 0;
                }
               
            }
            yield return null;
        }
        
    }
    void RotateTurret()
    {
        Vector3 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
        Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), turretRotationSpeed * Time.deltaTime);
        transform.rotation = finalRotation;
        
    }
    void Idle()
    {
        isDetecting = true;
    }
    void Attack()
    {
        if (target == null)
        {
            
            return;
        }
        if(Time.time >= nextAttack && target!= null)
        {
            nextAttack = Time.time + attackSpeed;

            GameObject go = Instantiate(bullet, shootingPoint.position, Quaternion.identity);
            go.GetComponent<Tower_Bullet>().SetBullet(target.transform, damage, bulletSpeed);
        }
    }
}
