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
        if(!unit.IsSkillCoolTimeOn)
        {
            Debug.Log("��ų ��Ÿ�����Դϴ�");
            return;
        }

        unit.UseSkill();
    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }
}
