using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Bullet : MonoBehaviour
{
    [SerializeField] float lifeSpan;
    [SerializeField] float acceleration;

    float speed;
    float damage;
    Vector2 direction;

    float timeAlive;

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        
        timeAlive += Time.deltaTime;
        if(timeAlive > lifeSpan) Die();
        
        
    }
    void FixedUpdate()
    {
        if(rb.velocity.magnitude < speed)
        {
            rb.velocity += direction * acceleration;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
    public void SetBullet(Transform target, float damage, float speed)
    {
        rb = GetComponent<Rigidbody2D>();

        this.damage = damage;
        this.speed = speed;

        direction = (target.position - transform.position).normalized;
        rb.velocity = direction * acceleration;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Melee_Enemy>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
