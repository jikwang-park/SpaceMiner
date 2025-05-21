using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageEndWindow : MonoBehaviour
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
            return gameObject.activeInHierarchy && endTime <= Time.time;
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
    }

    public void ShowStageEndMessage(bool cleared)
    {
        gameObject.SetActive(true);

        showTime = 2f;
        endTime = Time.time + showTime;
        fadeout = false;

        if (cleared)
        {
            SoundManager.Instance.PlaySFX("StageClearSFX");
            text.SetString(Defines.PlanetStageClearStringID);
        }
        else
        {
            SoundManager.Instance.PlaySFX("StageFailSFX");
            text.SetString(Defines.PlanetStageFailStringID);
        }
    }

    public void ShowPlanetStageMode(bool ascend)
    {
        gameObject.SetActive(true);

        showTime = 2f;
        endTime = Time.time + showTime;
        fadeout = false;

        if (ascend)
        {
            text.SetString(Defines.StageAscendModeStringID);
        }
        else
        {
            text.SetString(Defines.StageRepeatModeStringID);
        }
    }
}
