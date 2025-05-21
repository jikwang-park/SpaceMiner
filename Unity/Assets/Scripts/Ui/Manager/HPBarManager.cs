using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarManager : MonoBehaviour
{
    [SerializeField]
    private string hpbarAddress = "Assets/Addressables/Prefabs/UI/MonsterHPBar.prefab";

    private ObjectPoolManager objectPoolManager;
    private StageManager stageManager;

    [SerializeField]
    private Transform hpRect;

    private HashSet<IObjectPoolGameObject> monsterHPBars = new HashSet<IObjectPoolGameObject>();

    private void Awake()
    {
        objectPoolManager = GetComponent<ObjectPoolManager>();
    }

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        stageManager.OnIngameStatusChanged += OnIngameStatusChanged;
    }

    private void OnIngameStatusChanged(IngameStatus status)
    {
        ClearHPBar();
    }

    public void ClearHPBar()
    {
        while (monsterHPBars.Count > 0)
        {
            var enumerator = monsterHPBars.GetEnumerator();
            if (enumerator.MoveNext())
            {
                enumerator.Current.Release();
            }
        }
    }

    public void RemoveHPBar(IObjectPoolGameObject obj)
    {
        monsterHPBars.Remove(obj);
    }

    public void SetHPBar(Transform target)
    {
        var hpbarGo = objectPoolManager.Get(hpbarAddress);
        hpbarGo.transform.SetParent(hpRect);
        var hpbar = hpbarGo.GetComponent<MonsterHPBar>();
        hpbar.SetTarget(target, this);
        monsterHPBars.Add(hpbar);
    }
}
