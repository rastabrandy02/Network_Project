using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    Slider healthbar;
    [SerializeField] Transform parent;
    [SerializeField] Vector3 offset;

    Camera cam;


    void Start()
    {
        cam = Camera.main;
        healthbar = GetComponentInChildren<Slider>();       
    }

    public void SetHealth(float health, float maxHealth)
    {
        healthbar.value = health / maxHealth;
    }
    void Update()
    {             
        transform.rotation = cam.transform.rotation;
        transform.position = parent.position + offset;
    }
}
