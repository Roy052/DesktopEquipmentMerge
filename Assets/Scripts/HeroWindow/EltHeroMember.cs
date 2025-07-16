using UnityEngine;
using UnityEngine.Events;

public class EltHeroMember : EltHero
{
    public UnityAction<int> funcClick;
    public GameObject objInExpedition;
    public GameObject objInInjury;

    int idx;

    public void Set(InfoHero info, int idx)
    {
        base.Set(info);
        this.idx = idx;
    }

    public void OnClick()
    {
        funcClick?.Invoke(idx);
    }
}
