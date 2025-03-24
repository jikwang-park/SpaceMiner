using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMergePanelUI : MonoBehaviour
{
    [SerializeField]
    private SoldierInfoUI soldierInfoUI;
    [SerializeField]
    private SoldierInteractableUI interactableUI;

    public void Initialize(InventoryElement currentElement, InventoryElement nextElement)
    {
        soldierInfoUI.Initialize(currentElement);
        interactableUI.Initialize(currentElement, nextElement);
    }
}
