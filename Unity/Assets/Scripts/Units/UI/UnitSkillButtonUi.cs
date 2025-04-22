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
    private void ButtonUpdate()
    {
        if(unit.IsDead)
        {
            SkillButton.interactable = false;
            target.SetActive(false);
            return;
        }

        target.SetActive(true);
        if (unit.isAutoSkillMode)
        {
            SkillButton.interactable = true;
            
        }
        else if(!unit.IsSkillCoolTimeOn)
        {
            SkillButton.interactable = false;
        }
        else
        {
            SkillButton.interactable = true;
        }
        if (!unit.IsSkillCoolTimeOn && unit.isAutoSkillMode)
        {
            SkillButton.interactable = false;
        }
    }
    private void ShowCooltime()
    {
        if (unit.IsSkillCoolTimeOn)
        {
            skillCoolImage.fillAmount = 0f;
            coolTimeText.text = null;
        }
        else
        {
            skillCoolImage.fillAmount = 1.0f - unit.RemainSkillCoolTime;
            coolTimeText.text = unit.unitSkill.remainCooltime.ToString("F1");
        }
    }
    private void OnClickSkill()
    {
        if (unit.UnitTypes == UnitTypes.Dealer)
        {
            if(unit.targetDistance <= unit.unitStats.range)
            {
                unit.UseSkill();
            }
            if (unit.IsSkillCoolTimeOn && !unit.isAutoSkillMode)
            {
                unit.UseSkill();
            }
        }
        else
        {
            if (unit.IsSkillCoolTimeOn)
            {
                Debug.Log("수동 스킬 사용!");
                unit.UseSkill();
            }
        }
        
        //if(unit.IsSkillCoolTimeOn)
        //{
        //    unit.isSkillButtonPressed = true;
        //    unit.UseSkill();
        //    unit.isSkillButtonPressed = false;
        //}
    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }
}
