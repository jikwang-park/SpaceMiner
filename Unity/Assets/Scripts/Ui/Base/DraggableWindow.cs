using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 startWindowPosition;
    private Vector2 startMousePosition;
    private Vector2 moveOffset;

    private CursorLockMode prevCursorLockMode;

    private void StopDrag()
    {
        Cursor.lockState = prevCursorLockMode;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        prevCursorLockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Confined;

        startWindowPosition = transform.position;
        startMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        moveOffset = eventData.position - startMousePosition;
        transform.position = startWindowPosition + moveOffset;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            StopDrag();
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            StopDrag();
        }
#endif
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopDrag();
    }
}
