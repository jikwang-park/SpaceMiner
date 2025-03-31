using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button openSoldierManagePanelButton;
    [SerializeField]
    private GameObject soldierManagePanel;

    [SerializeField]
    private Button openUnitStatsManagePanelButton;
    [SerializeField]
    private GameObject unitStatsManagePanel;
    // Start is called before the first frame update
    void Start()
    {
        openSoldierManagePanelButton.onClick.AddListener(() => soldierManagePanel.SetActive(true));
        
        openUnitStatsManagePanelButton.onClick.AddListener(()=> unitStatsManagePanel.SetActive(true));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
