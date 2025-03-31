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

    //���߿� ���� �Ұ�
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
        statsInformation.text = $"���ݷ� ���� \n + {addValue}";
        statsUp += 1;
        gold += 200;
    }

    private void OnClickAddStatsButton()
    {
        // ���� ���� ������ư
        SetStatsInfo();



    }
}
