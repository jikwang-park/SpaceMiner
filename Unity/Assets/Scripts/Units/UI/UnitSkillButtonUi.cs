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

    private int spriteId;
    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        SkillButton.onClick.AddListener(() => OnClickSkill());
    }
    private void Update()
    {
        ShowCooltime();
        ButtonUpdate();
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
            target.SetActive(false);
            return;
        }

        target.SetActive(true);

        SkillButton.interactable = unit.Skill.SkillCoolTimeRatio >= 1f;

    }
    private void ShowCooltime()
    {
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
