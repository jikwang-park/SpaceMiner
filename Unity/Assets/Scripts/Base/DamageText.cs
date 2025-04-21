using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamageText : MonoBehaviour, IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }
    private StageManager stageManager;
    private TextMeshProUGUI text;
    private RectTransform rectTransform;
    private float showingDuration;
    private float risingSpeed;

    private readonly static Vector2 defaultPosition = Vector2.up * 10f;

    [SerializeField]
    private Color hitColor;
    [SerializeField]
    private Color critColor;
    [SerializeField]
    private Vector3 startOffset;

    private Vector3 startPos;
    private float startTime;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        stageManager.damageTexts.AddLast(this);
    }

    private void Update()
    {
        if (Time.time > startTime + showingDuration)
        {
            rectTransform.anchoredPosition = defaultPosition;
            Release();
            return;
        }
        Vector3 pos = startPos;
        pos.y += risingSpeed * (Time.time - startTime);
        transform.position = Camera.main.WorldToScreenPoint(pos);
    }

    public void Release()
    {
        stageManager.damageTexts.Remove(this);
        ObjectPool.Release(gameObject);
    }

    public void SetText(Attack attack)
    {
        text.text = attack.damage.ToString();
        text.color = attack.isCritical ? critColor : hitColor;
    }

    public void SetSpeed(float duration, float speed)
    {
        showingDuration = duration;
        risingSpeed = speed;
    }

    public void Show(Vector3 startPos)
    {
        this.startPos = startPos + startOffset;
        startTime = Time.time;
    }
}
