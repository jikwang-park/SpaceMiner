using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillButtonUi : MonoBehaviour
{
    [SerializeField]
    public Image skillCoolImage;

    [SerializeField]
    public Button SkillButton;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Unit unit;

    private StageManager stageManager;

    [SerializeField]
    private TextMeshProUGUI coolTimeText;

    [SerializeField]
    private AddressableImage skillImage;

    [SerializeField]
    private readonly Color deafaultColor = Color.white;
    [SerializeField]
    private readonly Color changedColor = new Color(1f, 1f, 1f, 0.3f);

    private int spriteId;
    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        SkillButton.onClick.AddListener(() => OnClickSkill());
        stageManager.UnitPartyManager.OnUnitCreated += OnUnitRespawn;
    }
    private void Update()
    {
        ShowCooltime();
        ButtonUpdate();
    }

    private void OnUnitRespawn()
    {
        skillImage.gameObject.GetComponent<Image>().color = deafaultColor;
        skillCoolImage.gameObject.SetActive(true);
    }

    public void SetSkillImage(UnitTypes type, Grade grade)
    {
        var skillDic = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId;

        switch (type)
        {
            case UnitTypes.Tanker:
                var tankerSkillId = skillDic[type][grade];
                spriteId = DataTableManager.TankerSkillTable.GetData(tankerSkillId).SpriteID;
                skillImage.SetSprite(spriteId);
                break;
            case UnitTypes.Dealer:
                var dealerSkillId = skillDic[type][grade];
                spriteId = DataTableManager.DealerSkillTable.GetData(dealerSkillId).SpriteID;
                skillImage.SetSprite(spriteId);
                break;
            case UnitTypes.Healer:
                var healerSkillId = skillDic[type][grade];
                spriteId = DataTableManager.HealerSkillTable.GetData(healerSkillId).SpriteID;
                skillImage.SetSprite(spriteId);
                break;
        }

    }
    private void ButtonUpdate()
    {
        if (unit.UnitStatus == Unit.Status.Dead)
        {
            SkillButton.interactable = false;
            skillImage.gameObject.GetComponent<Image>().color = changedColor;
            coolTimeText.text = null;
            return;
        }


        SkillButton.interactable = unit.Skill.SkillCoolTimeRatio >= 1f;

    }
    private void ShowCooltime()
    {
        if(unit.UnitStatus == Unit.Status.Dead)
        {
            skillCoolImage.gameObject.SetActive(false);
        }
        if (unit.Skill.SkillCoolTimeRatio >= 1f)
        {
            skillCoolImage.fillAmount = 0f;
            coolTimeText.text = null;
        }
        else
        {
            skillCoolImage.fillAmount = 1.0f - unit.Skill.SkillCoolTimeRatio;
            coolTimeText.text = unit.Skill.RemainCooltime.ToString("F1");
        }
    }
    private void OnClickSkill()
    {
        if (unit is null)
        {
            return;
        }
        unit.EnqueueSkill();
    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
        SetSkillImage(unit.UnitTypes, unit.Grade);
    }
}
