using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugStageView : MonoBehaviour
{
    private const string stageFormat = "{0}-{1}";


    [SerializeField]
    private TextMeshProUGUI currentStageText;
    [SerializeField]
    private TextMeshProUGUI clearedStageText;
    [SerializeField]
    private TextMeshProUGUI unlockedStageText;

    [SerializeField]
    private TMP_InputField currentStageInput;
    [SerializeField]
    private TMP_InputField clearedStageInput;
    [SerializeField]
    private TMP_InputField unlockedStageInput;

    private StageManager stageManager;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        RefreshText();
    }

    public void RefreshText()
    {
        var stageData = SaveLoadManager.Data.stageSaveData;

        currentStageText.text = string.Format(stageFormat, stageData.currentPlanet, stageData.currentStage);
        clearedStageText.text = string.Format(stageFormat, stageData.clearedPlanet, stageData.clearedStage);
        unlockedStageText.text = string.Format(stageFormat, stageData.highPlanet, stageData.highStage);
    }

    private bool TryParse(string str, out int planet, out int stage)
    {
        planet = 0;
        stage = 0;
        var splitedString = str.Split('-');

        if (splitedString.Length != 2)
        {
            return false;
        }
        if (!int.TryParse(splitedString[0], out int parsedPlanet))
        {
            return false;
        }
        if (!int.TryParse(splitedString[1], out int parsedStage))
        {
            return false;
        }
        if (!DataTableManager.StageTable.IsExistStage(parsedPlanet, parsedStage))
        {
            return false;
        }
        planet = parsedPlanet;
        stage = parsedStage;

        return true;
    }

    public void SetCurrentStage()
    {
        string temp = currentStageInput.text;
        currentStageInput.text = string.Empty;
        if (!TryParse(temp, out int planet, out int stage))
        {
            return;
        }
        var stageLoadData = SaveLoadManager.Data.stageSaveData;
        stageLoadData.currentPlanet = planet;
        stageLoadData.currentStage = stage;

        stageManager.SetStatus(IngameStatus.Planet);
        stageManager.StageUiManager.IngameUIManager.StageSelectWindow.gameObject.SetActive(false);
        stageManager.ResetStage();
        RefreshText();
    }

    public void SetClearedStage()
    {
        string temp = clearedStageInput.text;
        clearedStageInput.text = string.Empty;
        if (!TryParse(clearedStageInput.text, out int planet, out int stage))
        {
            return;
        }
        var stageLoadData = SaveLoadManager.Data.stageSaveData;
        stageLoadData.clearedPlanet = planet;
        stageLoadData.clearedStage = stage;
        RefreshText();
    }

    public void SetUnlockedStage()
    {
        string temp = unlockedStageInput.text;
        unlockedStageInput.text = string.Empty;
        if (!TryParse(unlockedStageInput.text, out int planet, out int stage))
        {
            return;
        }
        var stageLoadData = SaveLoadManager.Data.stageSaveData;
        stageLoadData.highPlanet = planet;
        stageLoadData.highStage = stage;
        RefreshText();
    }
}
