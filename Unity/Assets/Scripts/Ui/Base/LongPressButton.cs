using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnLongPress;
    public UnityEvent OnLongPressEnd;

    [SerializeField]
    private float minTime = 0.2f;
    [SerializeField]
    private float interval = 0.1f;

    private float targetTime = float.MaxValue;
    private bool isPressing;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressing = true;
        targetTime = Time.time + minTime;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnLongPressEnd?.Invoke();
        isPressing = false;
        targetTime = float.MaxValue;
    }

    void Update()
    {
        if (!isPressing)
        {
            return;
        }
        float currentTime = Time.time;

        if (currentTime > targetTime)
        {
            targetTime = currentTime + interval;
            OnLongPress?.Invoke();
        }
    }
}
