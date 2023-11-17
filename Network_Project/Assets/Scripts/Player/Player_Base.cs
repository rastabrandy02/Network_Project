using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Base : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] Healthbar healthbar;

    float health;
    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        healthbar.SetHealth(health, maxHealth);

        if(health <= 0)
        {
            Die();
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    void Die()
    {
        Debug.Log("Base destroyed!");
        Destroy(gameObject);
    }
}
