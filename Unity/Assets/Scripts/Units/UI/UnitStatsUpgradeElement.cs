using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsUpgradeElement : MonoBehaviour
{
    private BigNumber attackUp = 200;

    private BigNumber healthUp;

    private BigNumber defenseUp;

    private float criticalPrecentUp;

    private float cirticalDamageUp;

    private BigNumber gold = 100;

    private float statsUp = 1f;

    private BigNumber goldUp;

    private float maxLevel;

    [SerializeField]
    private Button addStatButton;
    [SerializeField]
    private TextMeshProUGUI addStartButtonText;
    [SerializeField]
    private TextMeshProUGUI statsInformation;
    [SerializeField]
    private UnitPartyManager unitPartyManager;

    //나중에 삭제 할것
    [SerializeField]
    private BigNumber addValue;

    private void Awake()
    {
        addStartButtonText.text = $"+ {gold} ";
        addStatButton.onClick.AddListener(() => OnClickAddStatsButton());
        
    }


    private void Init()
    {

    }

    private void SetStatsInfo()
    {
        addValue = attackUp * statsUp;
        statsInformation.text = $"공격력 증가 \n + {addValue}";
        statsUp += 1;
        gold += 200;
    }

    private void OnClickAddStatsButton()
    {
        // 스텟 정보 업데이튼
        SetStatsInfo();



    }
}
