using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class PlanetButton : MonoBehaviour, IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }

    [SerializeField]
    private TextMeshProUGUI text;

    [field: SerializeField]
    public Button Button { get; private set; }

    private int planet;

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
        this.planet = planet;
        text.text = $"{planet}";
    }
}
