using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class StageButton : MonoBehaviour, IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }

    [SerializeField]
    private TextMeshProUGUI text;

    private int planet;
    private int stage;

    [field: SerializeField]
    public Button Button { get; private set; }
    private StageManager stageManager;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    public void Release()
    {
        Button.interactable = false;
        ObjectPool.Release(gameObject);
    }

    public void Set(int planet, int stage)
    {
        this.planet = planet;
        this.stage = stage;
        text.text = $"{planet}-{stage}";
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
