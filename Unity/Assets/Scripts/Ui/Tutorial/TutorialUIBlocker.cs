using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIBlocker : MonoBehaviour
{
    public enum Position
    {
        Up,
        Down,
        Left,
        Right,
    }

    [SerializeField]
    private RectTransform top;

    [SerializeField]
    private RectTransform left;

    [SerializeField]
    private RectTransform right;

    [SerializeField]
    private RectTransform bottom;

    private RectTransform canvasRect;

    [SerializeField]
    private RectTransform arrow;

    private RectTransform targetRect;

    public bool isShowing { get; private set; }

    private Position arrowPosition;

    private Vector2 screenSize;

#if UNITY_EDITOR
    [SerializeField]
    private RectTransform debugTarget;

    [SerializeField]
    private bool useDebug;
#endif

    private void Update()
    {
#if UNITY_EDITOR
        if (useDebug)
        {
            Rect rect = GetWorldRect(debugTarget);
            SetBlock(rect);
        }
        else
        {
            Rect rect = GetWorldRect(targetRect);
            SetBlock(rect);
        }
#endif

        if (screenSize.x != canvasRect.rect.size.x)
        {
            screenSize = canvasRect.rect.size;
            Rect rect = GetWorldRect(targetRect);
            SetBlock(rect);
            ShowArrow(arrowPosition);
        }
    }

    private void Awake()
    {
        canvasRect = GetComponent<RectTransform>();
        screenSize = canvasRect.rect.size;
    }

    private void SetBlock(Rect rect)
    {
        top.offsetMin = new Vector2(0f, rect.yMax);

        left.offsetMax = new Vector2(-(canvasRect.rect.width - rect.xMin), -(canvasRect.rect.height - rect.yMax));
        left.offsetMin = new Vector2(0f, rect.yMin);

        right.offsetMax = new Vector2(0f, -(canvasRect.rect.height - rect.yMax));
        right.offsetMin = new Vector2(rect.xMax, rect.yMin);

        bottom.offsetMax = new Vector2(0f, -(canvasRect.rect.height - rect.yMin));
    }

    public void SetBlock(RectTransform rectTransform)
    {
        if (isShowing)
        {
            return;
        }
        isShowing = true;
        gameObject.SetActive(true);
        targetRect = rectTransform;
        Rect rect = GetWorldRect(targetRect);
        SetBlock(rect);
    }

    private Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector3 topLeft = corners[0];
        topLeft.x /= canvasRect.lossyScale.x;
        topLeft.y /= canvasRect.lossyScale.y;

        Vector2 scaledSize = new Vector2(rectTransform.rect.size.x, rectTransform.rect.size.y);
        Rect rect = new Rect(topLeft, scaledSize);
        return rect;
    }

    public void ShowArrow(Position arrowPosition)
    {
        var rect = GetWorldRect(targetRect);
        this.arrowPosition = arrowPosition;
        switch (arrowPosition)
        {
            case Position.Up:
                arrow.anchoredPosition = new Vector2(rect.center.x, rect.yMax);
                arrow.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case Position.Down:
                arrow.anchoredPosition = new Vector2(rect.center.x, rect.yMin);
                arrow.rotation = Quaternion.Euler(0f, 0f, 270f);
                break;
            case Position.Left:
                arrow.anchoredPosition = new Vector2(rect.xMin, rect.center.y);
                arrow.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case Position.Right:
                arrow.anchoredPosition = new Vector2(rect.xMax, rect.center.y);
                arrow.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        isShowing = false;
    }
}
