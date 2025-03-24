using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class StageButton : MonoBehaviour, IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }

    [SerializeField]
    private TextMeshProUGUI text;

    private int planet;
    private int stage;

    public void Release()
    {
        ObjectPool.Release(gameObject);
    }

    public void Set(int planet, int stage)
    {
        this.planet = planet;
        this.stage = stage;
        text.text = $"{planet}-{stage}";
    }
}
