using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleSpriteChange : MonoBehaviour
{
    [SerializeField]
    private Sprite onSprite;

    [SerializeField]
    private Sprite offSprite;

    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        ChangeSprite(toggle.isOn);
    }

    public void ChangeSprite(bool isOn)
    {
        if (isOn)
        {
            toggle.image.sprite = onSprite;
        }
        else
        {
            toggle.image.sprite = offSprite;
        }
    }
}
