using UnityEngine;
using UnityEngine.Events;

public class EltHeroExpeditionMember : EltHero
{
    public GameObject objSelected;
    bool isSelect = false;

    UnityAction<int, bool> funcClick;
    int idx = 0;
    public void Set(InfoHero info, int idx, UnityAction<int, bool> funcClick)
    {
        base.Set(info);
        this.idx = idx;
        this.funcClick = funcClick;

        objSelected.SetActive(false);
        isSelect = false;
    }

    public void OnClick()
    {
        isSelect = !isSelect;
        objSelected.SetActive(isSelect);
        funcClick?.Invoke(idx, isSelect);
    }
}
