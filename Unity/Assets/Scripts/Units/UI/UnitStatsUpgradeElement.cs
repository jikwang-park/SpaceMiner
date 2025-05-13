using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnitUpgradeTable;

public class UnitStatsUpgradeElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static readonly Color disabledColor = new Color(1f, 1f, 1f, 0.3f);


    public UpgradeType currentType;

    public float value;

    public BigNumber gold;

    public int maxLevel;
    // 아직 테이블에 없음
    public int level;

    public int nextLevel = 0;


    [SerializeField]
    private TextMeshProUGUI needGoldText;
    [SerializeField]
    private LocalizationText goldText;
    [SerializeField]
    private StageManager stageManager;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private int currentNum = 0;

    [SerializeField]
    private TextMeshProUGUI beforeStatsInfo;
    [SerializeField]
    private TextMeshProUGUI afterStatsInfo;
    [SerializeField]
    private LocalizationText titleText;

    [SerializeField]
    private AddressableImage goldImage;
    private Image goldimage;

    [SerializeField]
    private AddressableImage statsImage;
    //[SerializeField]
    //private Image goldImage;
    [SerializeField]
    private Image buttonImage;

    [SerializeField]
    private Transform buttonTransform;

    private Vector3 defaultScale = Vector3.one;
    private Vector3 pressedScale = new Vector3(0.9f, 0.9f, 1.0f);
    [SerializeField]
    private GameObject target;

    private int statsMultiplier = 1;

    private const string maxLevelString = "Max Level";

    [SerializeField]
    private BigNumber currentNoneCriValue = 0;
    [SerializeField]
    private float currentCritValue = 0f;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        ItemManager.OnItemAmountChanged += ItemManager_OnItemAmountChanged;
    }

    private void Start()
    {
        goldimage = goldImage.GetComponent<Image>();
    }

    public void SetMultiplier(int multiplier)
    {
        statsMultiplier = multiplier;
        SetStatsInfo();
    }

    public void SetInitString(UpgradeType type)
    {
        var stringId = DataTableManager.UnitUpgradeTable.GetData(type).NameStringID;
        titleText.SetString(stringId);
    }

    public void SetInitImage(UpgradeType type)
    {
        var spriteId = DataTableManager.UnitUpgradeTable.GetData(type).SpriteID;
        statsImage.SetSprite(spriteId);
        goldImage.SetSprite(10000);
    }


    private void SetStatsInfo()
    {
        nextLevel = Mathf.Min(maxLevel, level + statsMultiplier);
        levelText.text = $"Level + {level} \n-> + {nextLevel}";

        switch (currentType)
        {
            case UpgradeType.AttackPoint:
                BigNumber beforeAttackValue = UnitCombatPowerCalculator.GetAccountUpgradeAttackStat(level);
                BigNumber afterAttackValue = UnitCombatPowerCalculator.GetAccountUpgradeAttackStat(nextLevel);
                beforeStatsInfo.text = $"{beforeAttackValue}";
                afterStatsInfo.text = $"{afterAttackValue}";
                break;
            case UpgradeType.HealthPoint:
            case UpgradeType.DefensePoint:
                BigNumber beforeValue = GetCurrentValue(level);
                BigNumber afterValue = GetCurrentValue(nextLevel);
                beforeStatsInfo.text = $"{beforeValue:F2}";
                afterStatsInfo.text = $"{afterValue:F2}";
                break;
            case UpgradeType.CriticalPossibility:
            case UpgradeType.CriticalDamages:
                float beforeCriValue = GetCurrentCritValue(level);
                float afterCriValue = GetCurrentCritValue(nextLevel);
                beforeStatsInfo.text = $"{(beforeCriValue * 100f):F2}%";
                afterStatsInfo.text = $"{((afterCriValue) * 100f):F2}%";
                break;
        }
        if (level >= maxLevel)
        {
            needGoldText.text = maxLevelString;
        }
        else
        {
            BigNumber neededGold = GetUpgradeCost(level, statsMultiplier);
            ButtonUpdate(neededGold);

            needGoldText.text = $" +{neededGold}";
        }

    }
    public BigNumber GetStatUpgradeCost(int level)
    {
        if (level <= 0)
            return 0;

        if (level <= 100)
        {
            return 300 * Mathf.Pow(1.05f, level - 1);
        }
        else if (level <= 1000)
        {
            BigNumber cost = 300 * Mathf.Pow(1.05f, 99);
            return cost * Mathf.Pow(1.005f, level - 100);
        }
        else
        {
            BigNumber cost = 300 * Mathf.Pow(1.05f, 99) * Mathf.Pow(1.005f, 900);
            return cost * Mathf.Pow(1.001f, level - 1000);
        }
    }

    public BigNumber GetCriticalUpgradeCost(int level)
    {
        if (level <= 0)
            return 0;

        if (level <= 10)
        {
            return 300 * Mathf.Pow(1.05f, 10 * (level - 1));
        }
        else if (level <= 100)
        {
            BigNumber cost = 300 * Mathf.Pow(1.05f, 10 * (10 - 1));
            return cost * Mathf.Pow(1.005f, 10 * (level - 10));
        }
        else
        {
            BigNumber level10Cost = 300 * Mathf.Pow(1.05f, 10 * (10 - 1));
            BigNumber level100Cost = level10Cost * Mathf.Pow(1.005f, 10 * (100 - 10));
            return level100Cost * Mathf.Pow(1.001f, 10 * (level - 100));
        }
    }

    private BigNumber GetUpgradeCost(int level)
    {
        //if (upgradeStatsCost.TryGetValue(level, out var chachedCost))
        //    return chachedCost;

        BigNumber cost;


        if (currentType == UpgradeType.CriticalPossibility || currentType == UpgradeType.CriticalDamages)
        {
            cost = GetCriticalUpgradeCost(level);
        }
        else
        {
            cost = GetStatUpgradeCost(level);
        }
        //upgradeStatsCost[level] = cost;
        return cost;
    }

    private void ItemManager_OnItemAmountChanged(int itemID, BigNumber amount)
    {
        if (itemID != (int)Currency.Gold)
        {
            return;
        }
        BigNumber totalGold = GetUpgradeCost(level, statsMultiplier);

        ButtonUpdate(totalGold);
    }

    private BigNumber GetUpgradeCost(int startLevel, int length)
    {
        BigNumber cost = 0;
        if (startLevel + length > maxLevel)
        {
            length = maxLevel - startLevel;
        }
        switch (currentType)
        {
            case UpgradeType.AttackPoint:
            case UpgradeType.HealthPoint:
            case UpgradeType.DefensePoint:
                if (startLevel <= 100)
                {
                    int min = Mathf.Min(startLevel + length, 101);
                    int sumlength = min - startLevel;
                    cost = 300 * Mathf.Pow(1.05f, startLevel - 1) * ((1 - Mathf.Pow(1.05f, sumlength)) / (1 - 1.05f));

                    if (startLevel + length > 101)
                    {
                        length = startLevel + length - 101;
                        startLevel = 101;
                    }
                }
                if (startLevel > 100 && startLevel <= 1000)
                {
                    int min = Mathf.Min(startLevel + length, 1001);
                    int sumlength = min - startLevel;
                    cost += 300 * Mathf.Pow(1.05f, 99) * Mathf.Pow(1.005f, startLevel - 100) * ((1 - Mathf.Pow(1.005f, sumlength)) / (1 - 1.005f));

                    if (startLevel + length > 1001)
                    {
                        length = startLevel + length - 1001;
                        startLevel = 1001;
                    }
                }
                if (startLevel > 1000)
                {
                    cost += 300 * Mathf.Pow(1.05f, 99) * Mathf.Pow(1.005f, 900) * Mathf.Pow(1.001f, startLevel - 1000) * ((1 - Mathf.Pow(1.001f, length)) / (1 - 1.001f));
                }
                break;
            case UpgradeType.CriticalPossibility:
            case UpgradeType.CriticalDamages:

                if (startLevel <= 10)
                {
                    int min = Mathf.Min(startLevel + length, 11);
                    int sumlength = min - startLevel;
                    float pow105 = Mathf.Pow(1.05f, 10);
                    cost = 300 * Mathf.Pow(pow105, startLevel - 1) * ((1 - Mathf.Pow(pow105, sumlength)) / (1 - pow105));

                    if (startLevel + length > 11)
                    {
                        length = startLevel + length - 11;
                        startLevel = 11;
                    }
                }
                if (startLevel > 10 && startLevel <= 100)
                {
                    int min = Mathf.Min(startLevel + length, 101);
                    int sumlength = min - startLevel;
                    float pow105 = Mathf.Pow(1.05f, 10);
                    float pow1005 = Mathf.Pow(1.005f, 10);
                    cost += 300 * Mathf.Pow(pow105, 9) * Mathf.Pow(pow1005, startLevel - 10) * ((1 - Mathf.Pow(pow1005, sumlength)) / (1 - pow1005));

                    if (startLevel + length > 101)
                    {
                        length = startLevel + length - 101;
                        startLevel = 101;
                    }
                }
                if (startLevel > 100)
                {
                    float pow105 = Mathf.Pow(1.05f, 10);
                    float pow1005 = Mathf.Pow(1.005f, 10);
                    float pow1001 = Mathf.Pow(1.001f, 10);
                    cost += 300 * Mathf.Pow(pow105, 9) * Mathf.Pow(pow1005, 90) * Mathf.Pow(pow1001, startLevel - 100) * ((1 - Mathf.Pow(pow1001, length)) / (1 - pow1001));
                }
                break;
        }

        return cost;
    }


    public BigNumber GetGoldForMultipleLevels(int currentLevel, int multiplier)
    {

        BigNumber result = 0;
        int end = Mathf.Min(currentLevel + multiplier, maxLevel);

        for (int i = currentLevel + 1; i <= end; ++i)
        {
            //if(!upgradeGoldCost.TryGetValue(i, out var cost))
            //{
            //    cost = GetUpgradeCost(i);
            //    upgradeGoldCost[i] = cost;
            //}
            //result += cost;
        }

        return result;
    }
    public void SetData(int level, UnitUpgradeTable.Data data)
    {
        currentType = data.Type;
        value = data.Value;
        gold = data.NeedItemCount;
        maxLevel = data.MaxLevel;


        this.level = level;
        this.level = Mathf.Clamp(level, 1, maxLevel);

        if (currentType == UpgradeType.CriticalDamages || currentType == UpgradeType.CriticalPossibility)
        {
            currentCritValue = GetCurrentCritValue(level);
        }
        else
        {
            currentNoneCriValue = GetCurrentValue(level);
        }

        SetStatsInfo();
    }

    public BigNumber GetCurrentValue(int level)
    {
        BigNumber result = 0;
        if (currentType == UpgradeType.AttackPoint)
        {
            result = UnitCombatPowerCalculator.GetAccountUpgradeAttackStat(level);
        }
        else if (currentType == UpgradeType.HealthPoint || currentType == UpgradeType.DefensePoint)
        {
            result = value * level;
        }
        return result;
    }
    public float GetCurrentCritValue(int level)
    {
        float result = 0;
        if (currentType == UpgradeType.CriticalDamages || currentType == UpgradeType.CriticalPossibility)
        {
            result = value * level;
        }
        return result;
    }
    public BigNumber GetCurrentGold(int level)
    {
        BigNumber result = 0;
        for (int i = 1; i <= level; ++i)
        {
            result += GetUpgradeCost(i);
        }
        return result;
    }

    private void LevelUp()
    {
        if (level > maxLevel)
            return;

        int addLevel = statsMultiplier;


        level += addLevel;


        if (level > maxLevel)
        {
            level = maxLevel;
        }

        if (currentType == UpgradeType.CriticalDamages || currentType == UpgradeType.CriticalPossibility)
        {
            currentCritValue = value * level;
        }
        else
        {
            currentNoneCriValue = GetCurrentValue(level);
        }

        SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[currentType] = level;
        GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.StatUpgrade);

    }

    private bool CanUpgrade(BigNumber totalGold)
    {
        if (level >= maxLevel)
            return false;

        return ItemManager.CanConsume((int)Currency.Gold, totalGold);
    }

    private void ButtonUpdate(BigNumber totalGold)
    {
        if (needGoldText is null
            || goldimage is null)
        {
            return;
        }

        if (ItemManager.CanConsume((int)Currency.Gold, totalGold))
        {
            needGoldText.color = Color.white;
            goldText.SetColor(Color.white);
            goldimage.color = Color.white;
            buttonImage.color = Color.white;
        }
        else
        {
            if (needGoldText.color == disabledColor)
            {
                return;
            }

            needGoldText.color = disabledColor;
            goldText.SetColor(disabledColor);
            goldimage.color = disabledColor;
            buttonImage.color = disabledColor;
        }
    }

    private void OnClickAddStatsButton(BigNumber totalGold)
    {
        if (ItemManager.CanConsume((int)Currency.Gold, totalGold))
        {
            ItemManager.ConsumeCurrency(Currency.Gold, totalGold);
            LevelUp();
            SetStatsInfo();
            stageManager.UnitPartyManager.AddStats(currentType, value * statsMultiplier);
            SaveLoadManager.SaveGame();
        }
    }
    private bool isLongPressed = false;
    private Coroutine longPressedCor = null;
    private float longPressedDealyTime = 0.7f;
    private float longPressedReapeatDealyTime = 0.1f;
    private float pressedStartTime = 0f;
    private bool isPressing = false;

    private bool IsPointerOnButton(PointerEventData eventData)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(buttonImage.rectTransform, eventData.position, eventData.pressEventCamera);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsPointerOnButton(eventData))
            return;

        buttonTransform.localScale = pressedScale;
        isPressing = true;
        pressedStartTime = Time.time;
        if (longPressedCor == null)
        {
            longPressedCor = StartCoroutine(LongPressedCoroutine());
        }


    }

    public void OnPointerUp(PointerEventData eventData)
    {


        buttonTransform.localScale = defaultScale;
        isPressing = false;
        if (longPressedCor != null)
        {
            StopCoroutine(longPressedCor);
            longPressedCor = null;
        }

        if (Time.time - pressedStartTime < longPressedDealyTime)
        {
            BigNumber totalGold = GetUpgradeCost(level, statsMultiplier);
            if (CanUpgrade(totalGold))
            {
                OnClickAddStatsButton(totalGold);
            }
        }
    }

    private IEnumerator LongPressedCoroutine()
    {
        yield return new WaitForSeconds(longPressedDealyTime);

        bool doneUpgrade = false;
        BigNumber totalGold = GetUpgradeCost(level, statsMultiplier);
        while (CanUpgrade(totalGold))
        {
            if (ItemManager.CanConsume((int)Currency.Gold, totalGold))
            {
                ItemManager.ConsumeCurrency(Currency.Gold, totalGold);
                LevelUp();
                SetStatsInfo();
                stageManager.UnitPartyManager.AddStats(currentType, value * statsMultiplier);
                doneUpgrade = true;
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(longPressedReapeatDealyTime);
            totalGold = GetUpgradeCost(level, statsMultiplier);
        }

        if (doneUpgrade)
        {
            SaveLoadManager.SaveGame();
        }
    }
}
