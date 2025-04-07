using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSkillUpgradeManager : MonoBehaviour
{
    private UnitTypes currentType;

    private Grade currentGrade;

    private int id;

    public Dictionary<UnitTypes, Dictionary<Grade, int>> unitSkillDictionary = new Dictionary<UnitTypes, Dictionary<Grade, int>>();
    [SerializeField]
    private UnitSkillUpgradeBoard board;


    public UnitSkillGradeButtons gradeButtons;

    public void SetGradeButtons()
    {

        for(int i = (int)Grade.Normal; i<=(int)Grade.Legend; ++i)
        {
            var results = InventoryManager.IsExist(currentType, (Grade)i);
            gradeButtons.SetButton((Grade)i , results);
        }
    }
    private void Start()
    {
        foreach (UnitTypes type in Enum.GetValues(typeof(UnitTypes)))
        {
            foreach (Grade grade in Enum.GetValues(typeof(Grade)))
            {
                if (!unitSkillDictionary.ContainsKey(type))
                {
                    unitSkillDictionary.Add(type, new Dictionary<Grade, int>());
                }
                //테이블 수정 요구사항
                switch (type)
                {
                    case UnitTypes.Tanker:
                        unitSkillDictionary[type].Add(grade, (int)grade * 1000 + (int)type * 200 + 1);
                        break;
                    case UnitTypes.Dealer:
                        unitSkillDictionary[type].Add(grade, (int)grade * 1000 + (int)type * 50 + 1);
                        break;
                    case UnitTypes.Healer:
                        unitSkillDictionary[type].Add(grade, (int)grade * 1000 + (int)type * 100 + 1);
                        break;
                }
            }
        }
       
    }
    public int GetCurrentId()
    {
        id = unitSkillDictionary[currentType][currentGrade];
        return id;
    }
    public void SetType(UnitTypes type)
    {
        if (currentType == type)
            return;
        currentType = type;
        SetGrade(Grade.Normal);
        SetGradeButtons();
        board.SetInfo(GetCurrentId(),currentType);
    }

    private void OnEnable()
    {
        SetType(UnitTypes.Tanker);
    }
    public void SetGrade(Grade grade)
    {
        if (currentGrade == grade)
            return;

        currentGrade = grade;
        board.SetInfo(GetCurrentId(), currentType);
    }
}
