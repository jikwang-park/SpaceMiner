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

        id = unitSkillDictionary[currentType][currentGrade];
    }

   
    private void Start()
    {
        board.ShowFirstOpened(id, currentType,currentGrade);

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
        board.SetBoardText(GetCurrentId(), currentType,currentGrade);
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
        board.SetBoardText(GetCurrentId(), currentType, currentGrade);
    }
}
