using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiningRobotInventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    private MiningRobotIcon iconImage;
    [SerializeField]
    private List<Sprite> gradeSprites;
    [SerializeField]
    private int equipSlotIndex = -1;
    public bool IsEmpty { get; private set; } = true;
    public int index { get; set; }
    private int miningRobotId;

    private float holdThreshold = 0.1f;
    private Coroutine holdCoroutine;
    private bool isDragging = false;
    private Canvas parentCanvas;
    private GameObject dragIcon;
    private RectTransform dragIconRect;
    private Vector3 originalPosition;
    private ScrollRect parentScrollRect;
    private SlotType slotType;
    private void Awake()
    {
        parentScrollRect = GetComponentInParent<ScrollRect>();
    }
    public void Initialize(MiningRobotInventorySlotData data)
    {
        IsEmpty = data.isEmpty;
        slotType = data.slotType;
        if (!IsEmpty)
        {
            miningRobotId = data.miningRobotId;
        }
        else
        {
            miningRobotId = 0;
        }
        iconImage.Initialize(data.grade);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!IsEmpty)
        {
            originalPosition = eventData.position;
            holdCoroutine = StartCoroutine(HoldTimeCoroutine());
        }
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
        if(isDragging && dragIconRect != null)
        {
            dragIconRect.position = eventData.position;
        }
        else if(slotType == SlotType.Inventory)
        {
            parentScrollRect.OnDrag(eventData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!isDragging && slotType == SlotType.Inventory)
        {
            parentScrollRect.OnBeginDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(isDragging)
        {
            EndDragging(eventData);
        }
        else if(slotType == SlotType.Inventory)
        {
            parentScrollRect.OnEndDrag(eventData);
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

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        if(draggedObject != null)
        {
            MiningRobotInventorySlot draggedSlot = draggedObject.GetComponent<MiningRobotInventorySlot>();
            if(draggedSlot == null || draggedSlot.IsEmpty)
            {
                return;
            }

            if(draggedSlot.slotType == SlotType.Inventory && this.slotType == SlotType.Inventory)
            {
                MiningRobotInventoryManager.ProcessSlots(draggedSlot.index, this.index);
            }
            else if(draggedSlot.slotType == SlotType.Equip && this.slotType == SlotType.Inventory)
            {
                MiningRobotInventoryManager.SwapInventoryAndEquipmentSlot(this.index, draggedSlot.equipSlotIndex);
            }
            else if(draggedSlot.slotType == SlotType.Inventory && this.slotType == SlotType.Equip)
            {
                MiningRobotInventoryManager.SwapInventoryAndEquipmentSlot(draggedSlot.index, this.equipSlotIndex);
            }
        }
    }
}
