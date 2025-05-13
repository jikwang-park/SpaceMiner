using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonRequirementWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject oneButton;

    [SerializeField]
    private GameObject twoButtons;

    [SerializeField]
    private LocalizationText messageText;

    public void OpenNeedPlanet(int planet)
    {
        gameObject.SetActive(true);
        twoButtons.SetActive(false);
        oneButton.SetActive(true);
        messageText.SetString(Defines.RequirementsFailPlanet, planet.ToString());
    }

    public void OpenNeedPower(BigNumber power)
    {
        gameObject.SetActive(true);
        twoButtons.SetActive(false);
        oneButton.SetActive(true);
        messageText.SetString(Defines.RequirementsFailPower, power.ToString());
    }

    public void OpenNeedKey()
    {
        gameObject.SetActive(true);
        twoButtons.SetActive(true);
        oneButton.SetActive(false);
        messageText.SetString(Defines.RequirementsFailKey);
    }

    public void OpenMiningFullCount()
    {
        gameObject.SetActive(true);
        twoButtons.SetActive(false);
        oneButton.SetActive(true);
        messageText.SetString(Defines.RequirementsFailKey);
    }
}
