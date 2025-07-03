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
        // ȭ���ĵ���� ��ǥ ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            e.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos);

        offset = localPos - rt.anchoredPosition;   // ó�� ������ �������� �Ÿ� ����
    }

    // �巡�� �� ��� ȣ��
    public void OnDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            e.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos);

        rt.anchoredPosition = localPos - offset;   // ������ �ȼ����� �������� �̵�
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
