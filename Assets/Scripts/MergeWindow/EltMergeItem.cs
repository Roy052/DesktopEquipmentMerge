using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EltMergeItem : EltItem, IPointerDownHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityAction<int> funcMerge;

    Canvas canvas;
    GraphicRaycaster gr;
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    RectTransform rectTransform;
    Vector3 originalPosition;
    bool isDragged = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        gr = canvas.GetComponent<GraphicRaycaster>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GetMergeItemId() == -1)
            return;

        originalPosition = rectTransform.anchoredPosition;
        isDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GetMergeItemId() == -1)
            return;

        rectTransform.anchoredPosition += eventData.delta / GetCanvasScale();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GetMergeItemId() == -1)
            return;

        bool isFound = false;
        raycastResults.Clear();
        gr.Raycast(eventData, raycastResults);

        foreach (var res in raycastResults)
        {
            var target = res.gameObject.GetComponent<EltMergeItem>();
            if (target != null && target != this && target.GetMergeItemId() != -1)
            {
                OnMerge();
                target.OnMerge();
                isFound = true;
                break;
            }
        }

        // 3) �� ã������ �⺻ ��� ó��
        if (isFound == false)
        {
            rectTransform.anchoredPosition = originalPosition;
        }

        isDragged = false;
    }

    public void OnMerge()
    {
        funcMerge?.Invoke(idx);
    }

    // ĵ���� ������ ���� (ĵ������ Scale Mode: Scale With Screen Size �� �� �ʿ�)
    float GetCanvasScale()
    {
        var canvas = GetComponentInParent<Canvas>();
        return canvas ? canvas.scaleFactor : 1f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isDragged == false)
            OnClick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragged = false;
    }
}
