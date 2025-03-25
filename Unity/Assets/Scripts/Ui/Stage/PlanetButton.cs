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

    private Button button;

    private int planet;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Release()
    {
        button.onClick.RemoveAllListeners();
        ObjectPool.Release(gameObject);
    }

    public void Set(int planet)
    {
        this.planet = planet;
        text.text = $"{planet}";
    }
}
