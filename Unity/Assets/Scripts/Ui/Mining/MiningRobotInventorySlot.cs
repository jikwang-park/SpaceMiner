using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiningRobotInventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private List<Sprite> gradeSprites;
    public bool IsEmpty { get; private set; } = true;
    public int index { get; set; }
    private int miningRobotId;

    private float holdThreshold = 0.5f;
    private Coroutine holdCoroutine;
    private bool isDragging = false;
    private Canvas parentCanvas;
    private GameObject dragIcon;
    private RectTransform dragIconRect;
    private Vector3 originalPosition;
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void Initialize(MiningRobotInventorySlotData data)
    {
        IsEmpty = data.isEmpty;
        miningRobotId = data.miningRobotId;   
        if(!IsEmpty)
        {
            iconImage.color = Color.white;
            iconImage.sprite = gradeSprites[miningRobotId - 3001];
            image.raycastTarget = true;
        }
        else
        {
            iconImage.color = new Color(1, 1, 1, 0);
            image.raycastTarget = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originalPosition = eventData.position;
        holdCoroutine = StartCoroutine(HoldTimeCoroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
        if(isDragging)
        {
            EndDragging(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && dragIconRect != null)
        {
            dragIconRect.position = eventData.position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(isDragging)
        {
            EndDragging(eventData);
        }
    }
    private IEnumerator HoldTimeCoroutine()
    {
        float elapsed = 0f;
        while (elapsed < holdThreshold)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        StartDragging();
    }

    private void StartDragging()
    {
        if(IsEmpty)
        {
            return;
        }
        isDragging = true;
        parentCanvas = GetComponentInParent<Canvas>();
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(parentCanvas.transform, false);
        Image dragImage = dragIcon.AddComponent<Image>();
        dragImage.sprite = iconImage.sprite;
        // dragImage.SetNativeSize();
        dragIconRect = dragIcon.GetComponent<RectTransform>();
        dragIconRect.position = originalPosition;
        CanvasGroup canvasGroup = dragIcon.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        iconImage.color = new Color(1, 1, 1, 0.5f);
    }
    private void EndDragging(PointerEventData eventData)
    {
        isDragging = false;
        if(dragIcon != null)
        {
            Destroy(dragIcon);
            dragIcon = null;
        }
        iconImage.color = Color.white;

    }
}
