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


    public UnitSkillGradeToggles gradeToggle;


    private void Awake()
    {
        gradeToggle.Init();

        unitSkillDictionary = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId;

        id = unitSkillDictionary[currentType][currentGrade];

        InventoryManager.onChangedInventory += SetGradeToggles;
    }

   
    private void Start()
    {
        board.ShowFirstOpened(id, currentType,currentGrade);

    }
    public void SetGradeToggles()
    {

        for (int i = (int)Grade.Normal; i <= (int)Grade.Legend; ++i)
        {
            var results = InventoryManager.IsExist(currentType, (Grade)i);

            //gradeToggle.SetButton((Grade)i, results);
            gradeToggle.SetToggle((Grade)i, results);
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
        SetGradeToggles();
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
