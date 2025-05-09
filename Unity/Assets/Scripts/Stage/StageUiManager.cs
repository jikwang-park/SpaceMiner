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
    public TutorialPage TutorialWindow { get; private set; }

    [field: SerializeField]
    public GameObject ResourceRow { get; private set; }

    [field: SerializeField]
    public MessageWindow MessageWindow { get; private set; }

    [field: SerializeField]
    public UnitUiManager UnitUiManager { get; private set; }

    public event System.Action OnExitButtonClicked;

    private void Awake()
    {
        ObjectPoolManager = GetComponent<ObjectPoolManager>();
        HPBarManager = GetComponent<HPBarManager>();
        UnitUiManager = GetComponent<UnitUiManager>();
    }

    public void ExitButtonClicked()
    {
        OnExitButtonClicked?.Invoke();
    }
}
