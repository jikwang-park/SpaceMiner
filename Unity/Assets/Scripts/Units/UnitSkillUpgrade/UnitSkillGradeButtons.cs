using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillGradeButtons : MonoBehaviour
{
    [SerializeField]
    private Button normalButton;
    [SerializeField]
    private Button rareButton;
    [SerializeField]
    private Button epicButton;
    [SerializeField]
    private Button legendButton;
    [SerializeField]
    private UnitSkillUpgradeManager manager;


    private void Start()
    {
        normalButton.onClick.AddListener(() => OnClickNormalButton());
        rareButton.onClick.AddListener(() => OnClickRareButton());
        epicButton.onClick.AddListener(() => OnClickEpicButton());
        legendButton.onClick.AddListener(() => OnClickLegendButton());
    }

    private void OnClickNormalButton()
    {
        manager.SetGrade(Grade.Normal);
    }
    private void OnClickRareButton()
    {
        manager.SetGrade(Grade.Rare); 
    }
    private void OnClickEpicButton()
    {
        manager.SetGrade(Grade.Epic);
    }
    private void OnClickLegendButton()
    {
        manager.SetGrade(Grade.Legend);
    }
}
