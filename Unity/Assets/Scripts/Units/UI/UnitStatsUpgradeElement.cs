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
    private float currentValue = 0;
    [SerializeField]
    private BigNumber currentGold = 0;

    [SerializeField]
    private TextMeshProUGUI beforeStatsInfo;
    [SerializeField]
    private TextMeshProUGUI afterStatsInfo;
    [SerializeField]
    private LocalizationText titleText;
    [SerializeField]
    private Image StatsImage;
    [SerializeField]
    private Image goldImage;
    [SerializeField]
    private Image buttonImage;

    [SerializeField]
    private Transform buttonTransform;

    private Vector3 defaultScale = Vector3.one;
    private Vector3 pressedScale = new Vector3(0.9f, 0.9f, 1.0f);
    [SerializeField]
    private GameObject target;

    private int statsMultiplier = 1;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();

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
    //임시 처리
    public void SetImage(UpgradeType type, List<Sprite> statsSprite)
    {
        int index = (int)type;

        StatsImage.sprite = statsSprite[index - 1];
    }


    private void SetStatsInfo()
    {
        nextLevel = Mathf.Min(maxLevel, level + statsMultiplier);
        levelText.text = $"Level + {level} -> {nextLevel}";
        float beforeValue = level * value;
        float afterValue = nextLevel * value;

        BigNumber afterGold = GetCurrentGold(nextLevel);

        switch (currentType)
        {
            case UpgradeType.AttackPoint:
            case UpgradeType.HealthPoint:
            case UpgradeType.DefensePoint:
                beforeStatsInfo.text = $"{beforeValue:F2}";
                afterStatsInfo.text = $"{afterValue:F2}";
                break;
            case UpgradeType.CriticalPossibility:
            case UpgradeType.CriticalDamages:
                beforeStatsInfo.text = $"{(beforeValue * 100f):F2}%";
                afterStatsInfo.text = $"{((afterValue) * 100f):F2}%";
                break;
        }
        if (level >= maxLevel)
        {
            needGoldText.text = "Max Level";
        }
        else
        {
            BigNumber neededGold = GetGoldForMultipleLevels(level, statsMultiplier);

            needGoldText.text = $" +{neededGold}";
        }


    }
    public BigNumber GetGoldForMultipleLevels(int currentLevel, int multiplier)
    {
        int start = currentLevel + 1;
        int end = Mathf.Min(currentLevel + multiplier, maxLevel);

        BigNumber result = 0;
        for (int i = start; i <= end; ++i)
        {
            result += gold * i;
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
        this.level = Mathf.Clamp(level,1,maxLevel);

        currentValue = GetCurrentValue(level);
        currentGold = GetCurrentGold(level);
        SetStatsInfo();
    }

    public float GetCurrentValue(int level)
    {
        float result = 0;
        result = value * level;
        return result;
    }
    public BigNumber GetCurrentGold(int level)
    {
        BigNumber result = 0;
        for (int i = 1; i <= level; ++i)
        {
            result += gold * i;
        }
        return result;
    }
    private void Update()
    {
        ButtonUpdate();
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

        currentValue += level * value;
        currentGold += GetCurrentGold(level);


        SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[currentType] = level;
        GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.StatUpgrade);
    }

    private bool CanUpgrade()
    {
        if (level >= maxLevel)
            return false;

        BigNumber totalGold = GetGoldForMultipleLevels(level, statsMultiplier);
        return ItemManager.CanConsume((int)Currency.Gold, totalGold);
    }

    private void ButtonUpdate()
    {
        BigNumber totalGold = GetGoldForMultipleLevels(level, statsMultiplier);
        if (ItemManager.CanConsume((int)Currency.Gold, totalGold))
        {
            needGoldText.color = Color.white;
            goldText.SetColor(Color.white);
            goldImage.color = Color.white;
            buttonImage.color = Color.white;
        }
        else
        {
            needGoldText.color = new Color(1f, 1f, 1f, 0.3f);
            goldText.SetColor(new Color(1f, 1f, 1f, 0.3f));
            goldImage.color = new Color(1f, 1f, 1f, 0.3f);
            buttonImage.color = new Color(1f, 1f, 1f, 0.3f);
        }
    }

    private void OnClickAddStatsButton()
    {
        BigNumber totalGold = GetGoldForMultipleLevels(level, statsMultiplier);
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
    private float longPressedDealyTime = 1f;
    private float longPressedReapeatDealyTime = 0.2f;
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
            if (CanUpgrade())
            {
                OnClickAddStatsButton();
            }
        }
    }

    private IEnumerator LongPressedCoroutine()
    {
        yield return new WaitForSeconds(longPressedDealyTime);

        while (CanUpgrade())
        {
            BigNumber totalGold = GetGoldForMultipleLevels(level, statsMultiplier);
            if (ItemManager.CanConsume((int)Currency.Gold, totalGold))
            {
                ItemManager.ConsumeCurrency(Currency.Gold, totalGold);
                LevelUp();
                SetStatsInfo();
                stageManager.UnitPartyManager.AddStats(currentType, value * statsMultiplier);
                SaveLoadManager.SaveGame();
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(longPressedReapeatDealyTime);
        }
    }
}
