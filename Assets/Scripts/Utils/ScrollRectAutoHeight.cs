using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Scroll Rect�� Content ���̿� ���� preferredHeight�� �ڵ����� ����.
/// �θ� VerticalLayoutGroup�� Control/Force Expand�� ���� �� �ùٷ� Ȯ��˴ϴ�.
/// </summary>
[RequireComponent(typeof(ScrollRect), typeof(LayoutElement))]
public class ScrollRectAutoHeight : UIBehaviour, ILayoutSelfController
{
    [Tooltip("0 �����̸� ���� ����")]
    public float maxHeight = 0f;          // ���ϴ� �ִ� ���� (0 ���� = ���� ����)

    ScrollRect sr;
    LayoutElement le;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<ScrollRect>();
        le = GetComponent<LayoutElement>();
    }

    // ILayoutSelfController ���� �� ���̾ƿ� �н����� ȣ��
    public void SetLayoutHorizontal() => UpdateHeight();
    public void SetLayoutVertical() => UpdateHeight();

    void UpdateHeight()
    {
        if (!sr || !sr.content) return;

        float h = sr.content.sizeDelta.y;
        if (maxHeight > 0f) h = Mathf.Min(h, maxHeight);

        le.preferredHeight = h;  // �θ� VLG�� �� ���� ���
    }
}
