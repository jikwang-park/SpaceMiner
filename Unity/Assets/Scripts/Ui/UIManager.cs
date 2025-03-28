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
    // Start is called before the first frame update
    void Start()
    {
        openSoldierManagePanelButton.onClick.AddListener(() => soldierManagePanel.SetActive(true));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
