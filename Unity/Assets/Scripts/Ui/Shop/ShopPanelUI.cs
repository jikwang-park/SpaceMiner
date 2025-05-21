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
    private int currentIndex;
    private void Awake()
    {
        DisplayPanel((int)ShopTable.ShopType.DungeonKey);
    }
    private void DisplayPanel(int index)
    {
        int newIndex = index - 1;
        if (index <= 0 || index > shopPanels.Count)
        {
            return;
        }

        if (shopPanels[newIndex].activeSelf)
        {
            return;
        }

        if (currentIndex < shopPanels.Count)
        {
            shopPanels[currentIndex].SetActive(false);
        }
        shopPanels[currentIndex].SetActive(false);
        currentIndex = newIndex;
        shopPanels[currentIndex].SetActive(true);
    }
    public void OnClickOpenDungeonKeyShop()
    {
        DisplayPanel((int)ShopTable.ShopType.DungeonKey);
    }
    public void OnClickOpenGoldShop()
    {
        DisplayPanel((int)ShopTable.ShopType.Gold);
    }
    public void OnClickOpenMiningRobotShop()
    {
        DisplayPanel((int)ShopTable.ShopType.MiningRobot);
    }
}
