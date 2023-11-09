using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Bullet : MonoBehaviour
{
    [SerializeField] float lifeSpan;
    
    float speed;
    float damage;
    Vector2 direction;
    Transform target;

    float timeAlive;
    void Start()
    {
        
    }

    
    void Update()
    {
        
        
        transform.position += new Vector3(direction.x, direction.y, 0.0f) * speed * Time.deltaTime;
        
        
        timeAlive += Time.deltaTime;
        if(timeAlive > lifeSpan) Die();
        
        
    }

    void Die()
    {
        Destroy(gameObject);
    }
    public void SetBullet(Transform target, float damage, float speed)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;

        direction = (target.position - transform.position).normalized;
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
