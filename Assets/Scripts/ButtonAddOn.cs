using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Button))]
public class ButtonAddOn : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    readonly Vector3 scaleUp = new(1.05f, 1.05f, 1.05f);
    readonly Vector3 scaleDown = Vector3.one;

    public SFX soundClick = SFX.Click;
    public bool scaling = false;

    [NonSerialized] public Button btn;
    RectTransform rt;

    void Awake()
    {
        btn = GetComponent<Button>();
        rt = GetComponent<RectTransform>();
    }

    public void ChangeGrayScale(bool isGray)
    {
        if (isGray)
            btn.image.material = Singleton.resourceManager.mat_GrayScale;
        else
            btn.image.material = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(btn.enabled)
            Singleton.soundManager.PlaySFX(soundClick);
    }

    public void ToggleEnable(bool isOn)
    {
        btn.enabled = isOn;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (rt != null && scaling)
            rt.localScale = scaleUp;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (rt != null && scaling)
            rt.localScale = scaleDown;
    }
}
