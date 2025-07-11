using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameWindow : Singleton, IBeginDragHandler, IDragHandler
{
    public Vector2 PosOrigin;

    public bool IsShow => gameObject.activeSelf;

    RectTransform rt;
    Canvas canvas;
    Vector2 offset;

    protected virtual void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            e.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos);

        offset = localPos - rt.anchoredPosition;
    }

    // 드래그 중 계속 호출
    public void OnDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            e.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos);

        rt.anchoredPosition = localPos - offset;
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        if (rt.localPosition.x > 960f || rt.localPosition.x < -960f || rt.localPosition.y > 540f || rt.localPosition.y < -540f)
            rt.localPosition = PosOrigin;
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
