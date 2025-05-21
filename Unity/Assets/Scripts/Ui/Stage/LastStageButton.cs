using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class LastStageButton : MonoBehaviour
{
    [SerializeField]
    private LocalizationText buttonText;

    private int planet;
    private int stage;

    [field: SerializeField]
    public Button Button { get; private set; }
    private StageManager stageManager;

    [SerializeField]
    private AddressableImage planetImage;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        stageManager.OnStageEnd += OnStageEnd;
        OnStageEnd();
    }

    private void OnStageEnd()
    {
        planet = SaveLoadManager.Data.stageSaveData.highPlanet;
        stage = SaveLoadManager.Data.stageSaveData.highStage;

        var planetData = DataTableManager.PlanetTable.GetData(this.planet);
        var planetName = DataTableManager.StringTable.GetData(planetData.NameStringID);

        buttonText.SetStringArguments(planetName, planet.ToString(), stage.ToString());
        planetImage.SetSprite(planetData.SpriteID);
    }

    public void MoveStage()
    {
        SaveLoadManager.Data.stageSaveData.currentPlanet = planet;
        SaveLoadManager.Data.stageSaveData.currentStage = stage;

        stageManager.SetStatus(IngameStatus.Planet);
        stageManager.StageUiManager.IngameUIManager.StageSelectWindow.gameObject.SetActive(false);
        stageManager.ResetStage();

        //SaveLoadManager.SaveGame();
        //SceneManager.LoadScene(0);
        //Addressables.LoadSceneAsync("StageDevelopScene");
    }
}
