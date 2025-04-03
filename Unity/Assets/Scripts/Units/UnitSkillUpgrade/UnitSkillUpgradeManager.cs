using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillUpgradeManager : MonoBehaviour
{
    public UnitTypes currentType;

    public Grade currentGrade;

    private int id;

    public Dictionary<UnitTypes, Dictionary<Grade, int>> unitSkillDictionary = new Dictionary<UnitTypes, Dictionary<Grade, int>>();
    [SerializeField]
    private UnitSkillUpgradeBoard board;

    private UnitUpgradeTable.Data unitData;

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

                unitSkillDictionary[type].Add(grade, (int)grade * 1000 + (int)type * 100 + 1);
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
        board.SetInfo(GetCurrentId());
    }
    public void SetGrade(Grade grade)
    {
        if (currentGrade == grade)
            return;

        currentGrade = grade;
        board.SetInfo(GetCurrentId());

    }
}
