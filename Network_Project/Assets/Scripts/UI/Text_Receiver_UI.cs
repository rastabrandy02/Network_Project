using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Text_Receiver_UI : MonoBehaviour
{
    [SerializeField] UDPReceive udpReceiver;
    TMP_Text tmpText;
    void Start()
    {
        tmpText = GetComponent<TMP_Text>();
        tmpText.text = "Awaiting to receive data...";
    }

    void Update()
    {
        tmpText.text = udpReceiver.GetStoredData();
    }
   
}
