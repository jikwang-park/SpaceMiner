using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class StageUiManager : MonoBehaviour
{
    public ObjectPoolManager objectPoolManager { get; private set; }

    [field: SerializeField]
    public InGameUIManager ingameUIManager { get; private set; }

    [field: SerializeField]
    public InteractableUIManager interactableUIManager { get; private set; }

    [field: SerializeField]
    public ScreenCurtain curtain { get; private set; }

    private void Awake()
    {
        objectPoolManager = GetComponent<ObjectPoolManager>();
    }
}
