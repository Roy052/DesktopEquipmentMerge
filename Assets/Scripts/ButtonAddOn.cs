using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonAddOn : MonoBehaviour, IPointerClickHandler
{
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
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
        Singleton.soundManager.PlaySFX(SFX.Click);
    }
}
