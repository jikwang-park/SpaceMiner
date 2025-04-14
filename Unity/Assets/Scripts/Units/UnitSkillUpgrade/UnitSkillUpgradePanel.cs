using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSkillUpgradePanel : MonoBehaviour
{
    private UnitTypes currentType = UnitTypes.Tanker;

    private Grade currentGrade = Grade.Normal;

    private int id;



    public Dictionary<UnitTypes, Dictionary<Grade, int>> unitSkillDictionary = new Dictionary<UnitTypes, Dictionary<Grade, int>>();
    [SerializeField]
    private UnitSkillUpgradeBoard board;


    public UnitSkillGradeButtons gradeButtons;


    private void Awake()
    {
        gradeButtons.Init();

        unitSkillDictionary = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId;

        //foreach (UnitTypes type in Enum.GetValues(typeof(UnitTypes)))
        //{
        //    foreach (Grade grade in Enum.GetValues(typeof(Grade)))
        //    {
        //        currentType = type;
        //        currentGrade = grade;
        //        id = data[currentType][grade];

        //        if (!unitSkillDictionary.ContainsKey(type))
        //        {
        //            unitSkillDictionary.Add(type, new Dictionary<Grade, int>());
        //        }

        //        unitSkillDictionary[type].Add(grade, id);
        //    }
        //}

        id = unitSkillDictionary[currentType][currentGrade];
        board.ShowFirstOpened(id, currentType);
    }   
    public void SetGradeButtons()
    {

        for (int i = (int)Grade.Normal; i <= (int)Grade.Legend; ++i)
        {
            var results = InventoryManager.IsExist(currentType, (Grade)i);
            gradeButtons.SetButton((Grade)i, results);
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
        board.SetInfo(GetCurrentId(), currentType);
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
