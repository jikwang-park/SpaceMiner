using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillGradeToggles : MonoBehaviour
{
   

    [SerializeField]
    private Toggle normalToggle;
    [SerializeField]
    private Toggle rareToggle;
    [SerializeField]
    private Toggle epicToggle;
    [SerializeField]
    private Toggle legendToggle;

    [SerializeField]
    private Image normalToggleImage;
    [SerializeField]
    private Image rareToggleImage;
    [SerializeField]
    private Image epicToggleImage;
    [SerializeField]
    private Image legendToggleImage;
    private const float pressedToggleColor = 200f;

    [SerializeField]
    private Image normalFrame;
    [SerializeField]
    private Image rareFrame;
    [SerializeField]
    private Image epicFrame;
    [SerializeField]
    private Image legendFrame;


    private void OnEnable()
    {
        normalToggle.isOn = false;
        rareToggle.isOn = false;
        epicToggle.isOn = false;
        legendToggle.isOn = false;
        normalToggle.isOn = true;
    }
    public Dictionary<Grade, Toggle> toggleDic = new Dictionary<Grade, Toggle>();



    [SerializeField]
    private UnitSkillUpgradePanel panel;


  
    public void Init()
    {
 
        toggleDic.Add(Grade.Normal,normalToggle);
        toggleDic.Add(Grade.Rare,rareToggle);
        toggleDic.Add(Grade.Epic,epicToggle);
        toggleDic.Add(Grade.Legend,legendToggle);
        panel.SetGradeToggles();
    }
    public void SetToggle(Grade grade, bool result)
    {   
        toggleDic[grade].interactable = result;
    }

    public void UpdateSelectedFrame()
    {
        normalFrame.enabled = normalToggle.isOn;
        rareFrame.enabled = rareToggle.isOn;
        epicFrame.enabled = epicToggle.isOn;
        legendFrame.enabled = legendToggle.isOn;
    }

    public void OnClickToggle()
    {
        if(normalToggle.isOn)
        {
            panel.SetGrade(Grade.Normal);
        }
        else if(rareToggle.isOn)
        {
            panel.SetGrade(Grade.Rare);
        }
        else if(epicToggle.isOn)
        {
            panel.SetGrade(Grade.Epic);
        }
        else if(legendToggle.isOn)
        {
            panel.SetGrade(Grade.Legend);
        }
        UpdateSelectedFrame();

    }
 
}
