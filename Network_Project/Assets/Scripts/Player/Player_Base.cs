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

        if (health <= 0)
        {
            Die();
        }


    }
      
    public void SendDamage(float damage)
    {        
        health -= damage;
        
        BaseDmgPacket packet = new BaseDmgPacket();
        packet.damage = damage;
        OnlineManager.instance.SendPacket(packet);
    }
   
    public void SendDamageHost(float damage)
    {
        health -= damage;

        Debug.Log("Checking if BaseDMG is being SENT");
        BaseDmgPacket packet = new BaseDmgPacket();
        packet.damage = damage;
        packet.isHost = true;
        OnlineManager.instance.SendPacket(packet);
    }

    public void SendDamageClient(float damage)
    {
        health -= damage;

        BaseDmgPacket packet = new BaseDmgPacket();
        packet.damage = damage;
        packet.isHost = false;
        OnlineManager.instance.SendPacket(packet);
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
