using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmountSlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private LocalizationText amountText;

    private const int maxLevelStringId = 60010;
    private int currentValue;
    private int nextLevelValue;
    public void SetValue(int currentValue, int nextLevelValue)
    {
        if(this.currentValue == currentValue && this.nextLevelValue == nextLevelValue) 
        {
            return;
        }

        this.currentValue = currentValue;
        this.nextLevelValue = nextLevelValue;
        slider.value = (float)this.currentValue / this.nextLevelValue;
        if(nextLevelValue == 0)
        {
            amountText.SetString(maxLevelStringId);
        }
        else
        {
            amountText.SetStringArguments(this.currentValue.ToString(), this.nextLevelValue.ToString());
        }
    }
}
