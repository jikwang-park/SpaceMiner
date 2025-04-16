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
    private UnitSkillUpgradePanel panel;

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
        panel.SetGradeButtons();
    }
    public void SetButton(Grade grade, bool result)
    {
        buttonDictionary[grade].interactable = result;
    }

    private void OnClickNormalButton()
    {
        panel.SetGrade(Grade.Normal);
    }
    private void OnClickRareButton()
    {
        panel.SetGrade(Grade.Rare);
    }
    private void OnClickEpicButton()
    {
        panel.SetGrade(Grade.Epic);
    }
    private void OnClickLegendButton()
    {
        panel.SetGrade(Grade.Legend);
    }

  
}
