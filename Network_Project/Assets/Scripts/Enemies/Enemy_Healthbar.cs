using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Healthbar : MonoBehaviour
{
    Slider healthbar;
    [SerializeField] Transform parent;
    [SerializeField] Vector3 offset;

    Melee_Enemy enemyStats;
    Camera cam;


    void Start()
    {
        cam = Camera.main;
        healthbar = GetComponentInChildren<Slider>();
        enemyStats = GetComponentInParent<Melee_Enemy>();
    }


    void Update()
    {
        healthbar.value = enemyStats.GetHealth() / enemyStats.GetMaxHealth();
       
        transform.rotation = cam.transform.rotation;

        transform.position = parent.position + offset;


    }
}
