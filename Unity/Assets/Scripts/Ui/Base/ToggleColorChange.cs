using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleColorChange : MonoBehaviour
{
    [SerializeField]
    private Color onColor;

    [SerializeField]
    private Color offColor;

    private Toggle toggle;

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
            var colors = toggle.colors;
            colors.normalColor = onColor;
            toggle.colors = colors;
        }
        else
        {
            var colors = toggle.colors;
            colors.normalColor = offColor;
            toggle.colors = colors;
        }
    }
}
