using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MiningRobotInventoryManager;

public class MiningPanelUI : MonoBehaviour
{
    [SerializeField]
    private List<Toggle> planetButtons;
    [SerializeField]
    private Button moveButton;
    [SerializeField]
    private MiningRobotMergePopupUI robotMergePopupUI;
    [SerializeField]
    private SetMiningRobotToPlanetUI setRobotToPlanetUI;

    private List<int> planetIds = new List<int>();
    private StageManager stageManager;
    private StageSaveData stageSaveData;
    private void Awake()
    {
        stageSaveData = SaveLoadManager.Data.stageSaveData;
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        planetIds = DataTableManager.PlanetTable.GetIds();
        onRequestMerge += ShowMergePopup;
    }
    private void Start()
    {
        for (int i = 0; i < planetIds.Count; i++)
        {
            var spriteId = DataTableManager.PlanetTable.GetData(planetIds[i]).SpriteID;
            planetButtons[i].GetComponent<AddressableImage>().SetSprite(spriteId);
        }
    }
    private void ShowMergePopup(int mergedRobotId, MergeResponseCallback callback)
    {
        RobotMergeTable.Data mergeData = DataTableManager.RobotMergeTable.GetData(mergedRobotId);
        if (mergeData == null)
        {
            callback.Invoke(false);
            return;
        }
        robotMergePopupUI.Initialize(mergedRobotId, callback);
        robotMergePopupUI.gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        CheckPlanetsOpen();
        OnClickPlanetButton(Variables.planetMiningID - 1);
        planetButtons[Variables.planetMiningID - 1].isOn = true;
    }
    public void OnClickPlanetButton(int index)
    {
        setRobotToPlanetUI.Initialize(planetIds[index]);
        currentPlanetId = planetIds[index];
    }
    public void OnClickSortButton()
    {
        MiningRobotInventoryManager.SortInventorySlots();
    }

    public void OnClickMoveButton()
    {
        Variables.planetMiningID = currentPlanetId;
        stageManager.SetStatus(IngameStatus.Mine);
    }
    public void OnClickMoveButtonInMine()
    {
        Variables.planetMiningID = currentPlanetId;
        stageManager.ResetStage();
    }
    public void CheckPlanetsOpen()
    {
        var checkResults = MiningRobotInventoryManager.CheckPlanetsOpen();
        var openedPlanets = checkResults.Where((e) => e.Value == true).ToList();
        moveButton.interactable = openedPlanets.Count > 0;
        for (int i = 0; i < planetIds.Count; i++)
        {
            planetButtons[i].interactable = checkResults[planetIds[i]];
        }
    }
}
