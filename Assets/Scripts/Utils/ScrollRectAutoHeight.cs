using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Scroll Rect의 Content 길이에 맞춰 preferredHeight를 자동으로 갱신.
/// 부모 VerticalLayoutGroup이 Control/Force Expand를 켰을 때 올바로 확장됩니다.
/// </summary>
[RequireComponent(typeof(ScrollRect), typeof(LayoutElement))]
public class ScrollRectAutoHeight : UIBehaviour, ILayoutSelfController
{
    [Tooltip("0 이하이면 제한 없음")]
    public float maxHeight = 0f;          // 원하는 최대 높이 (0 이하 = 제한 없음)

    ScrollRect sr;
    LayoutElement le;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<ScrollRect>();
        le = GetComponent<LayoutElement>();
    }

    // ILayoutSelfController 구현 ― 레이아웃 패스마다 호출
    public void SetLayoutHorizontal() => UpdateHeight();
    public void SetLayoutVertical() => UpdateHeight();

    void UpdateHeight()
    {
        if (!sr || !sr.content) return;

        float h = sr.content.sizeDelta.y;
        if (maxHeight > 0f) h = Mathf.Min(h, maxHeight);

        le.preferredHeight = h;  // 부모 VLG가 이 값을 사용
    }
}
