using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] float attackSpeed;
    [SerializeField] float damage;
    [SerializeField] float detectionRange;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] LayerMask enemyLayerMask;

    float nextAttack;
    GameObject target;

    delegate void State();
    State state;
    void Start()
    {
        state = CheckTarget;
    }

    
    void Update()
    {
        state();
    }

    void CheckTarget()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayerMask);
       
        if (collisions != null)
        {
            
            float closestDistance = 999.9f;

            for(int i = 0; i< collisions.Length; i++) 
            {
                float currentDistance = Vector2.Distance(transform.position, collisions[i].transform.position);
                if (currentDistance < closestDistance)
                {
                    target = collisions[i].gameObject;
                    closestDistance = currentDistance;
                }
            }
           
            state = Attack;
        }
    }

    void Attack()
    {
        if (target == null)
        {
            state = CheckTarget;
            return;
        }
        if(Time.time >= nextAttack && target!= null)
        {
            nextAttack = Time.time + attackSpeed;

            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity);
            go.GetComponent<Tower_Bullet>().SetBullet(target.transform, damage, bulletSpeed);
        }
    }
}
