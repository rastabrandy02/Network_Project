using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] TMP_Text coinsTMP;
    [SerializeField] Player_Stats player_Stats;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinsTMP.text = player_Stats.GetCoins().ToString();
    }
}
