using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Text_Receiver_UI : MonoBehaviour
{
    TMP_Text tmpText;
    void Start()
    {
        tmpText = GetComponent<TMP_Text>();
        tmpText.text = "Awaiting to receive data...";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeText(string text)
    {
        tmpText.text = text;
    }
}
