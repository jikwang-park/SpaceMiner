using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillButtonUi : MonoBehaviour
{
    [SerializeField]
    public Image skillCoolImage;

    [SerializeField]
    public Button SkillButton;

    

    [SerializeField]
    private Unit unit;

    private StageManager stageManager;

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
        if ((!unit.isAutoSkillMode && !unit.isAutoSkillMode) || !unit.IsSkillCoolTimeOn)
        {
            SkillButton.interactable = false;
        }
        SkillButton.interactable = true;
    }
    private void ShowCooltime()
    {
        if (unit.IsSkillCoolTimeOn)
        {
            skillCoolImage.fillAmount = 0f;
        }
        else
        {
            skillCoolImage.fillAmount = 1.0f - unit.RemainSkillCoolTime;
        }
    }
    private void OnClickSkill()
    {
        if (!unit.isAutoSkillMode && unit.IsSkillCoolTimeOn)
        {
            Debug.Log("수동 스킬 사용!");
            unit.UseSkill();
        }
    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }
}
