using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillUpgradeManager : MonoBehaviour
{
    private UnitTypes currentType = UnitTypes.Tanker;

    private Grade currentGrade = Grade.Normal;

    private int id;

    public Dictionary<UnitTypes, Dictionary<Grade, int>> unitSkillDictionary = new Dictionary<UnitTypes, Dictionary<Grade, int>>();
    [SerializeField]
    private UnitSkillUpgradeBoard board;

   

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
       
        board.SetInfo(GetCurrentId(),currentType);
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
        board.SetInfo(GetCurrentId(),currentType);
    }
    public void SetGrade(Grade grade)
    {
        if (currentGrade == grade)
            return;

        currentGrade = grade;
        board.SetInfo(GetCurrentId(), currentType);

    }
}
