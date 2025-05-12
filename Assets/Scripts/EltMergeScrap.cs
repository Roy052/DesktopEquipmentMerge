using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EltMergeScrap : EltScrap, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Canvas canvas;
    GraphicRaycaster gr;
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    RectTransform rectTransform;
    Vector3 originalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        gr = canvas.GetComponent<GraphicRaycaster>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GetScrapType() == 0)
            return;

        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GetScrapType() == 0)
            return;

        rectTransform.anchoredPosition += eventData.delta / GetCanvasScale();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GetScrapType() == 0)
            return;

        bool isFound = false;
        raycastResults.Clear();
        gr.Raycast(eventData, raycastResults);

        foreach (var res in raycastResults)
        {
            var target = res.gameObject.GetComponent<EltMergeScrap>();
            if (target != null && target != this && target.GetScrapType() != 0)
            {
                OnClick();
                target.OnClick();
                isFound = true;
                break;
            }
        }

        // 3) 못 찾았으면 기본 드롭 처리
        if (isFound == false)
        {
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    // 캔버스 스케일 보정 (캔버스가 Scale Mode: Scale With Screen Size 일 때 필요)
    float GetCanvasScale()
    {
        var canvas = GetComponentInParent<Canvas>();
        return canvas ? canvas.scaleFactor : 1f;
    }

}
