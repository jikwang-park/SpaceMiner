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
    private StageManager stageManager;
    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }
    private void Start()
    {

        tankerButton.onClick.AddListener(() => OnClickTankerButton());
        dealerButton.onClick.AddListener(() => OnClickDealerButton());
        healerButton.onClick.AddListener(() => OnClickHealerButton());
    }

    private void SetButton()
    {

    }

    private void SetSkillUpgradeData(UnitTypes type)
    {
        switch (type)
        {
            case UnitTypes.Tanker:
                break;
            case UnitTypes.Dealer:
                break;
            case UnitTypes.Healer:
                break;
        }
    }


    private void OnClickTankerButton()
    {

    }
    
    private void OnClickDealerButton()
    {

    }
    private void OnClickHealerButton()
    {

    }
}
