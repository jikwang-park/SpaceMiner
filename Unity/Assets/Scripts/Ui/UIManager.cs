using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button openSoldierManagePanelButton;
    [SerializeField]
    private InventoryPanelUI soldierManagePanel;

    [SerializeField]
    private Button openUnitStatsManagePanelButton;
    [SerializeField]
    private GameObject unitStatsManagePanel;

    [SerializeField]
    private Button openGachaPanelButton;
    [SerializeField]
    private GachaInteractableUI gachaPanel;
    // Start is called before the first frame update
    void Start()
    {
        soldierManagePanel.Initialize();
        gachaPanel.Initialize();
        openSoldierManagePanelButton.onClick.AddListener(() => soldierManagePanel.gameObject.SetActive(true));
        
        openUnitStatsManagePanelButton.onClick.AddListener(()=> unitStatsManagePanel.SetActive(true));
        openGachaPanelButton.onClick.AddListener(() =>  gachaPanel.gameObject.SetActive(true));
    }
}
