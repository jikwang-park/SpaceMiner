using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionElement : MonoBehaviour
{
    [SerializeField]
    private Button upgradeButton;
    [SerializeField]
    private TextMeshProUGUI upgradeButtonText;
    [SerializeField]
    private Image constructionImage;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI constructionExpalnText;


    [SerializeField]
    private int level;


    private void Awake()
    {
        upgradeButton.onClick.AddListener(() => OnClickUpgradeButton());
    }

    public void Init()
    {

    }

    private void SetConstructionInfo()
    {

    }

    public void SetData()
    {

    }

    public float GetCurrentValue()
    {
        return 0;
    }

    public BigNumber GetCurrentGold()
    {
        return 1;
    }

    public void LevelUp()
    {

    }

    private void OnClickUpgradeButton()
    {

    }
}
