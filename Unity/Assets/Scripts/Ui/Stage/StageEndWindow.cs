using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageEndWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI messageText;

    private float targetTime = 0f;

    private void Update()
    {
        if (Time.time > targetTime)
        {
            Close();
        }
    }

    public void Open(string text, float duration)
    {
        messageText.text = text;
        gameObject.SetActive(true);
        targetTime = Time.time + duration;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
