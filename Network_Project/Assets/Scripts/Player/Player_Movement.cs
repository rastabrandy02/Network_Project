using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] Rigidbody2D rb;

    Vector2 movVec = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SendMovement());
    }

    void Update()
    {
        movVec = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
        {
            movVec.x = acceleration;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movVec.x = -acceleration;
        }
        if (Input.GetKey(KeyCode.W))
        {
            movVec.y = acceleration;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movVec.y = -acceleration;
        }
    }
    void FixedUpdate()
    {
        Move(movVec);
    }

    void Move(Vector2 movement)
    {
        if (rb.velocity.magnitude >= maxSpeed) return;

        rb.velocity += movement;
    }

    IEnumerator SendMovement()
    {
        while (true)
        {
            PlayerMovementPacket packet = new PlayerMovementPacket();
            packet.direction = movVec;
            OnlineManager.instance.SendPacket(packet);

            yield return new WaitForSeconds(0.15f);
        }
    }

}
