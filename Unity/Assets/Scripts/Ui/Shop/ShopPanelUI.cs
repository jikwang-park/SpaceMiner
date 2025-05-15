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

    private int currentIndex = -1;
    private void Start()
    {
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

    }
}
