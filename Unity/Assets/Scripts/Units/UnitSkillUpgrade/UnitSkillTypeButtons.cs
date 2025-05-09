using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSkillTypeButtons : MonoBehaviour
{
    [SerializeField]
    private UnitSkillGradeToggles toggle;
    [SerializeField]
    private UnitSkillUpgradePanel manager;
    [SerializeField]
    private Color defaultColor = Color.white;

    [SerializeField]
    private Toggle tankerToggle;
    [SerializeField]
    private Toggle dealerToggle;
    [SerializeField]
    private Toggle healerToggle;

    [SerializeField]
    private Sprite selectedSprite;
    [SerializeField]
    private Sprite deselectedSprite;

    [SerializeField]
    private Image tankerImage;
    [SerializeField]
    private Image dealerImage;
    [SerializeField]
    private Image healerImage;


  

    private void OnEnable()
    {
        tankerToggle.isOn = false;
        dealerToggle.isOn = false;
        healerToggle.isOn = false;
        tankerToggle.isOn = true;
    }


    public void OnClickToggle()
    {
        if (tankerToggle.isOn)
        {
            manager.SetType(UnitTypes.Tanker);
        }
        else if (dealerToggle.isOn)
        {
            manager.SetType(UnitTypes.Dealer);

        }
        else if (healerToggle.isOn)
        {
            manager.SetType(UnitTypes.Healer);
        }
        tankerImage.sprite = tankerToggle.isOn ? selectedSprite : deselectedSprite;
        dealerImage.sprite = dealerToggle.isOn ? selectedSprite : deselectedSprite;
        healerImage.sprite = healerToggle.isOn ? selectedSprite : deselectedSprite;
        toggle.toggleDic[Grade.Normal].isOn = true;
        toggle.OnClickToggle();
    }
}
