using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class StageUiManager : MonoBehaviour
{
    public ObjectPoolManager ObjectPoolManager { get; private set; }

    [field: SerializeField]
    public IngameUIManager IngameUIManager { get; private set; }

    [field: SerializeField]
    public UIGroupStatusManager UIGroupStatusManager { get; private set; }

    [field: SerializeField]
    public ScreenCurtain curtain { get; private set; }

    private void Awake()
    {
        ObjectPoolManager = GetComponent<ObjectPoolManager>();
    }
}
