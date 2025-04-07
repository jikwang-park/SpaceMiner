using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageEndWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI messageText;

    public void Open(string text)
    {
        messageText.text = text;
        gameObject.SetActive(true);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
