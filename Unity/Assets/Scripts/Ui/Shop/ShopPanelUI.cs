using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanelUI : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> shopPanels = new List<GameObject>();
    private int currentIndex;
    private void Awake()
    {
        currentIndex = 0;
        DisplayPanel(currentIndex);
    }

    private void DisplayPanel(int index)
    {
        shopPanels[currentIndex].SetActive(false);
        currentIndex = index;
        shopPanels[currentIndex].SetActive(true);
    }
    public void OnClickOpenKeyShop()
    {
    }
}
