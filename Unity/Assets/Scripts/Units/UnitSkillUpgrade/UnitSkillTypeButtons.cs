using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillTypeButtons : MonoBehaviour
{
    [SerializeField]
    public Button tankerButton;
    [SerializeField]
    public Button dealerButton;
    [SerializeField]
    public Button healerButton;
    [SerializeField]
    private UnitSkillUpgradeManager manager;
    
    private void Awake()
    {
        tankerButton.onClick.AddListener(() => OnClickTankerButton());
        dealerButton.onClick.AddListener(() => OnClickDealerButton());
        healerButton.onClick.AddListener(() => OnClickHealerButton());
    }
    private void Start()
    {
       
    }

    private void SetButton()
    {

    }



    private void OnClickTankerButton()
    {
        manager.SetType(UnitTypes.Tanker);
    }
    
    private void OnClickDealerButton()
    {
        manager.SetType(UnitTypes.Dealer);
    }
    private void OnClickHealerButton()
    {
        manager.SetType(UnitTypes.Healer);
    }
}
