using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StageSelectScroll : MonoBehaviour
{
    [SerializeField]
    private string buttonReference = "StageButton";

    [SerializeField]
    private Transform contents;

    private List<StageButton> buttons = new List<StageButton>();

    private ObjectPoolManager objectpoolManager;

    private StageSaveData stageLoadData;

#if UNITY_EDITOR
    private bool debugMode = false;
#endif

    private void Start()
    {
        stageLoadData = SaveLoadManager.Data.stageSaveData;

        SetButtons(stageLoadData.currentPlanet);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            debugMode = !debugMode;
            foreach (var button in buttons)
            {
                button.Button.interactable = true;
            }
        }
    }
#endif

    public void SetButtons(int planet)
    {
        if (objectpoolManager is null)
        {
            objectpoolManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>().StageUiManager.objectPoolManager;
        }
        if (stageLoadData is null)
        {
            stageLoadData = SaveLoadManager.Data.stageSaveData;
        }

        foreach (var button in buttons)
        {
            button.Release();
        }
        buttons.Clear();

        var planetDatas = DataTableManager.StageTable.GetPlanetData(planet);
        for (int i = 0; i < planetDatas.Count; ++i)
        {
            var buttonGo = objectpoolManager.Get(buttonReference);
            buttonGo.transform.SetParent(contents);
            buttonGo.transform.localScale = Vector3.one;
            var button = buttonGo.GetComponent<StageButton>();
            button.Set(planet, planetDatas[i].Stage);
            buttons.Add(button);
            if (planet < stageLoadData.highPlanet || (planet == stageLoadData.highPlanet && i < stageLoadData.highStage))
            {
                button.Button.interactable = true;
            }
#if UNITY_EDITOR
            else if (debugMode)
            {
                button.Button.interactable = true;
            }
#endif
        }
    }

    public void UnlockStage(int stage)
    {
        for (int i = 0; i < stage; ++i)
        {
            buttons[i].Button.interactable = true;
        }
    }
}
