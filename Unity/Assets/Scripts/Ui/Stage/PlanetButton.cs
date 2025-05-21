using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.UI;

public class PlanetButton : MonoBehaviour, IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }

    [SerializeField]
    private LocalizationText text;

    [field: SerializeField]
    public Button Button { get; private set; }

    [SerializeField]
    private AddressableImage planetImage;

    private GameObject planetModel;

    private int planet;

    private StageManager stageManager;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    public void Release()
    {
        Button.onClick.RemoveAllListeners();
        Button.interactable = false;
        ObjectPool.Release(gameObject);
    }

    public void Set(int planet)
    {
        if (stageManager is null)
        {
            stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        }

        this.planet = planet;
        var planetData = DataTableManager.PlanetTable.GetData(this.planet);
        var planetName = DataTableManager.StringTable.GetData(planetData.NameStringID);
        text.SetStringArguments(planet.ToString(), planetName);
        planetImage.SetSprite(planetData.SpriteID);
    }
}
