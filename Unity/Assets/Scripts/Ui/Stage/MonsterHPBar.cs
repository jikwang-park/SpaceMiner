using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour, IObjectPoolGameObject
{
    private Transform target;

    private Slider slider;

    [SerializeField]
    private Vector3 offset;

    public IObjectPool<GameObject> ObjectPool { get; set; }

    private HPBarManager hpbarManager;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        Vector3 targetPos = target.position;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos + offset);

        transform.position = screenPos;
    }

    public void Release()
    {
        hpbarManager.RemoveHPBar(this);
        ObjectPool.Release(gameObject);
    }

    private void OnTargetHPChanged(float value)
    {
        //gameObject.SetActive(true);
        slider.value = value;
    }

    private void OnTargetRelease(DestructedDestroyEvent sender)
    {
        Release();
    }

    public void SetTarget(Transform target,HPBarManager hpbarManager)
    {
        this.hpbarManager = hpbarManager;
        this.target = target;
        OnTargetHPChanged(1f);
        target.GetComponent<DestructedDestroyEvent>().OnDestroyed += OnTargetRelease;
        target.GetComponent<MonsterStats>().onHPChanged += OnTargetHPChanged;
        //gameObject.SetActive(false);
    }
}
