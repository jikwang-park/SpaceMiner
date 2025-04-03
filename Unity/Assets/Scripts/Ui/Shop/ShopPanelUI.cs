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
        if(index <= 0 || index > shopPanels.Count || currentIndex == index)
        {
            return;
        }
        shopPanels[currentIndex].SetActive(false);
        currentIndex = index - 1;
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
