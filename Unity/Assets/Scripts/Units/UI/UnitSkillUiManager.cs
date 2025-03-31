using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillUiManager : MonoBehaviour
{
    public GameObject[] skillObj;

    private StageManager stageManager;

    private List<Unit> units;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        
    }
    private void Start()
    {
        units = stageManager.UnitPartyManager.generateInstance;
    }
    private void Update()
    {
        SetUiStatus();
    }
    private void SetUiStatus()
    {
        foreach (Unit unit in units)
        {
            switch (unit.UnitTypes)
            {
                case UnitTypes.Tanker:
                    if(unit.unitSkill.coolTime != 0)
                    {
                        Color currentColor = skillObj[0].GetComponent<Image>().color;
                        currentColor.a = 0.5f;
                        skillObj[0].GetComponent<Image>().color = currentColor;
                    }
                    else
                    {
                        Color currentColor = skillObj[0].GetComponent<Image>().color;
                        currentColor.a = 1.0f;
                        skillObj[0].GetComponent<Image>().color = currentColor;
                    }
                    break;
                case UnitTypes.Dealer:
                    if (unit.unitSkill.coolTime != 0)
                    {
                        Color currentColor = skillObj[1].GetComponent<Image>().color;
                        currentColor.a = 0.5f;
                        skillObj[0].GetComponent<Image>().color = currentColor;
                    }
                    else
                    {
                        Color currentColor = skillObj[1].GetComponent<Image>().color;
                        currentColor.a = 1.0f;
                        skillObj[0].GetComponent<Image>().color = currentColor;
                    }
                    break;
                case UnitTypes.Healer:
                    if (unit.unitSkill.coolTime != 0)
                    {
                        Color currentColor = skillObj[2].GetComponent<Image>().color;
                        currentColor.a = 0.5f;
                        skillObj[0].GetComponent<Image>().color = currentColor;
                    }
                    else
                    {
                        Color currentColor = skillObj[2].GetComponent<Image>().color;
                        currentColor.a = 1.0f;
                        skillObj[0].GetComponent<Image>().color = currentColor;
                    }
                    break;
            }
        }
    }

}
