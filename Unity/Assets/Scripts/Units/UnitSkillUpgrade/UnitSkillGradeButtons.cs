using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Dictionary<Grade, Button> buttonDictionary = new Dictionary<Grade, Button>();

    private void Awake()
    {
        normalButton.onClick.AddListener(() => OnClickNormalButton());
        rareButton.onClick.AddListener(() => OnClickRareButton());
        epicButton.onClick.AddListener(() => OnClickEpicButton());
        legendButton.onClick.AddListener(() => OnClickLegendButton());
       
    }
    public void Init()
    {
        buttonDictionary.Add(Grade.Normal, normalButton);
        buttonDictionary.Add(Grade.Epic, epicButton);
        buttonDictionary.Add(Grade.Rare, rareButton);
        buttonDictionary.Add(Grade.Legend, legendButton);
    }
    public void SetButton(Grade grade, bool result)
    {
        buttonDictionary[grade].interactable = result;
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
