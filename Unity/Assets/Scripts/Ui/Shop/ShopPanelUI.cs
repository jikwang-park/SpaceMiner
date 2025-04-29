using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelUI : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> shopPanels = new List<GameObject>();
    [SerializeField]
    private Toggle keyShopToggle;
    [SerializeField]
    private Toggle robotShopToggle;
    [SerializeField]
    private Toggle goldShopToggle;
    [SerializeField]
    private Sprite selectedSprite;
    [SerializeField]
    private Sprite deselectedSprite;

    private Image keyShopToggleImage;
    private Image robotShopToggleImage;
    private Image goldShopToggleImage;
    private int currentIndex = -1;
    private void Awake()
    {
        keyShopToggleImage = keyShopToggle.GetComponent<Image>();
        robotShopToggleImage = robotShopToggle.GetComponent<Image>();
        goldShopToggleImage = goldShopToggle.GetComponent<Image>();
    }
    private void OnEnable()
    {
        keyShopToggle.isOn = false;
        robotShopToggle.isOn = false;
        goldShopToggle.isOn = false;
        keyShopToggle.isOn = true;
    }
    private void DisplayPanel(int index)
    {
        int newIndex = index - 1;
        if (newIndex < 0 || newIndex >= shopPanels.Count)
        {
            return;
        }

        if (currentIndex == newIndex && shopPanels[currentIndex].activeSelf)
        {
            return;
        }

        if (currentIndex >= 0 && currentIndex < shopPanels.Count)
        {
            shopPanels[currentIndex].SetActive(false);
        }

        currentIndex = newIndex;
        shopPanels[currentIndex].SetActive(true);
    }
    public void OnProcessToggles()
    {
        if(keyShopToggle.isOn)
        {
            DisplayPanel((int)ShopTable.ShopType.DungeonKey);
        }
        else if(robotShopToggle.isOn)
        {
            DisplayPanel((int)ShopTable.ShopType.MiningRobot);
        }
        else if(goldShopToggle.isOn)
        {
            DisplayPanel((int)ShopTable.ShopType.Gold);
        }

        UpdateToggleSprites();
    }
    private void UpdateToggleSprites()
    {
        keyShopToggleImage.sprite = keyShopToggle.isOn ? selectedSprite : deselectedSprite;
        robotShopToggleImage.sprite = robotShopToggle.isOn ? selectedSprite : deselectedSprite;
        goldShopToggleImage.sprite = goldShopToggle.isOn ? selectedSprite : deselectedSprite;
    }
}
