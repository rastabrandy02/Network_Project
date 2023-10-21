using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Server_UI_Text : MonoBehaviour
{
    [SerializeField] UDP_Server UDP_Server;
    TMP_Text tmpText;
    void Start()
    {
        tmpText = GetComponent<TMP_Text>();

    }

    void Update()
    {
        tmpText.text = UDP_Server.uiText;
    }



}
