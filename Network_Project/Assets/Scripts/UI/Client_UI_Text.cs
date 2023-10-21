using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Client_UI_Text : MonoBehaviour
{
    [SerializeField] UDP_Client udpClient;
    TMP_Text tmpText;
    void Start()
    {
        tmpText = GetComponent<TMP_Text>();
       
    }

    void Update()
    {
        tmpText.text = udpClient.uiText;
    }

}
