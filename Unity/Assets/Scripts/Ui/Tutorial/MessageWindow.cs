using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour
{
    [SerializeField]
    private Image background;

    [SerializeField]
    private float showTime = 1.5f;

    [SerializeField]
    private LocalizationText text;

    private float endTime;

    private bool fadeout;

    public bool isShowing
    {
        get
        {
            return endTime <= Time.time;
        }
    }

    private void Update()
    {
        float currentTime = Time.time;

        if (currentTime > endTime)
        {
            gameObject.SetActive(false);
        }

        if (!fadeout)
        {
            return;
        }

        var ratio = (endTime - currentTime) / showTime;

        Color tempColor = background.color;
        tempColor.a = ratio;
        background.color = tempColor;

        tempColor = text.color;
        tempColor.a = ratio;
        text.color = tempColor;
    }

    public void ShowStageRestrict(int planet, int stage)
    {
        gameObject.SetActive(true);

        showTime = 1.5f;
        endTime = Time.time + showTime;
        fadeout = true;

        text.SetString(Defines.RestrictionStringID, planet.ToString(), stage.ToString());
        Color tempColor = background.color;
        tempColor.a = 1f;
        background.color = tempColor;
        SoundManager.Instance.PlaySFX("AlertSFX");

        tempColor = text.color;
        tempColor.a = 1f;
        text.color = tempColor;
    }
}
