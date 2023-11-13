using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
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
   
}
