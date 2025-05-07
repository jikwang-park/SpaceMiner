using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class StageUiManager : MonoBehaviour
{
    public ObjectPoolManager ObjectPoolManager { get; private set; }
    public HPBarManager HPBarManager { get; private set; }

    [field: SerializeField]
    public IngameUIManager IngameUIManager { get; private set; }

    [field: SerializeField]
    public UIGroupStatusManager UIGroupStatusManager { get; private set; }

    [field: SerializeField]
    public ScreenCurtain curtain { get; private set; }

    [field: SerializeField]
    public Transform DamageParent { get; private set; }

    [field: SerializeField]
    public GameObject InteractableUIBackground { get; private set; }

    [field: SerializeField]
    public TutorialWindow TutorialWindow { get; private set; }

    public event System.Action OnExitButtonClicked;
    public event System.Action MiningBattleClicked;

    private void Awake()
    {
        ObjectPoolManager = GetComponent<ObjectPoolManager>();
        HPBarManager = GetComponent<HPBarManager>();
    }

    public void ExitButtonClicked()
    {
        OnExitButtonClicked?.Invoke();
    }

    public void MiningBattle()
    {
        MiningBattleClicked?.Invoke();
    }
}
