using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleColorChange : MonoBehaviour
{
    public enum ColorMode
    {
        Tint,
        Set,
    }

    [SerializeField]
    private Color onColor;

    [SerializeField]
    private Color offColor;

    private Toggle toggle;

    [SerializeField]
    private ColorMode colorMode;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(ChangeColor);
    }

    private void Start()
    {
        ChangeColor(toggle.isOn);
    }

    private void ChangeColor(bool isOn)
    {
        if (isOn)
        {
            if (colorMode == ColorMode.Tint)
            {
                var colors = toggle.colors;
                colors.normalColor = onColor;
                toggle.colors = colors;
            }
            else
            {
                toggle.image.color = onColor;
            }
        }
        else
        {
            if (colorMode == ColorMode.Tint)
            {
                var colors = toggle.colors;
                colors.normalColor = offColor;
                toggle.colors = colors;
            }
            else
            {
                toggle.image.color = offColor;
            }
        }
    }
}
