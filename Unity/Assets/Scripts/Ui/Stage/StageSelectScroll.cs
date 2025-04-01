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

    private void Start()
    {
        objectpoolManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>().stageUiManager.GetComponent<ObjectPoolManager>();

        SetButtons(Variables.planetNumber);
    }

    public void SetButtons(int planet)
    {
        foreach (var button in buttons)
        {
            button.Release();
        }
        buttons.Clear();

        var planetDatas = DataTableManager.StageTable.GetPlanetData(planet);
        for (int i = 0; i < planetDatas.Count; ++i)
        {
            var buttonGo = objectpoolManager.gameObjectPool[buttonReference].Get();
            buttonGo.transform.SetParent(contents);
            buttonGo.transform.localScale = Vector3.one;
            var button = buttonGo.GetComponent<StageButton>();
            button.Set(planet, planetDatas[i].Stage);
            buttons.Add(button);
            if (planet < Variables.maxPlanetNumber || (planet == Variables.maxPlanetNumber && i < Variables.maxStageNumber))
            {
                button.Button.interactable = true;
            }
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
