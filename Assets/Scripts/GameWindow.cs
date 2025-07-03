using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameWindow : Singleton, IBeginDragHandler, IDragHandler
{
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
        // 화면→캔버스 좌표 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            e.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos);

        offset = localPos - rt.anchoredPosition;   // 처음 눌렀던 지점과의 거리 저장
    }

    // 드래그 중 계속 호출
    public void OnDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            e.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos);

        rt.anchoredPosition = localPos - offset;   // “잡은 픽셀”을 기준으로 이동
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
