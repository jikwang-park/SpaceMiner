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
        if (unit.UnitStatus == Unit.Status.Dead)
        {
            SkillButton.interactable = false;
            target.SetActive(false);
            return;
        }

        target.SetActive(true);

        SkillButton.interactable = unit.SkillCoolTimeRatio >= 1f;

    }
    private void ShowCooltime()
    {
        if (unit.SkillCoolTimeRatio >= 1f)
        {
            skillCoolImage.fillAmount = 0f;
            coolTimeText.text = null;
        }
        else
        {
            skillCoolImage.fillAmount = 1.0f - unit.SkillCoolTimeRatio;
            coolTimeText.text = unit.RemainCooltime.ToString("F1");
        }
    }
    private void OnClickSkill()
    {
        if (unit is null)
        {
            return;
        }
        unit.EnqueueSkill();


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
