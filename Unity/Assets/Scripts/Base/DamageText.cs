using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamageText : MonoBehaviour, IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }
    private StageManager stageManager;
    private TextMeshPro text;
    private float showingDuration;
    private float risingSpeed;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        stageManager.damageTexts.AddLast(this);
    }

    public void Release()
    {
        stageManager.damageTexts.Remove(this);
        ObjectPool.Release(gameObject);
    }

    public void SetText(Attack attack)
    {
        text.text = attack.damage.ToString();
        text.color = attack.isCritical ? Color.red : Color.blue;
    }

    public void SetSpeed(float duration, float speed)
    {
        showingDuration = duration;
        risingSpeed = speed;
    }

    public void Show()
    {
        StartCoroutine(CoShow());
    }

    private IEnumerator CoShow()
    {
        float timer = 0f;
        while (timer < showingDuration)
        {
            timer += Time.deltaTime;
            var pos = text.transform.position;
            pos.y += risingSpeed * Time.deltaTime;
            text.transform.position = pos;
            yield return null;
        }
        Release();
    }
}
