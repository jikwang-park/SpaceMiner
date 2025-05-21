using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryWindow : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> panels = new List<GameObject>();
    [SerializeField]
    private List<Toggle> toggles = new List<Toggle>();
    [SerializeField]
    private Sprite selectedSprite;
    [SerializeField]
    private Sprite deselectedSprite;

    private List<Image> toggleImages = new List<Image>();
    void Awake()
    {
        for(int i = 0; i < toggles.Count; i++)
        {
            toggleImages.Add(toggles[i].GetComponent<Image>());
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            toggles[i].isOn = false;
        }
        toggles[0].isOn = true;
        ProcessToggles();
    }
    public void ProcessToggles()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            panels[i].SetActive(toggles[i].isOn);
        }
        UpdateToggles();
    }
    public void UpdateToggles()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            toggleImages[i].sprite = toggles[i].isOn ? selectedSprite : deselectedSprite;
        }
    }
}
