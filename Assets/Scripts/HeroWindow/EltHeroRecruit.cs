using UnityEngine;
using UnityEngine.UI;

public class EltHeroRecruit : EltHero
{
    public Text textBtnHeroPrice;
    public Image imgItem;
    int idx;

    public void Set(InfoHero info, int idx)
    {
        base.Set(info);
        this.idx = idx;
        textBtnHeroPrice.text = $"°í¿ë {info.price}";
    }

    public void OnClickRecruit()
    {
        Singleton.gm.gameData.RecruitUnit(idx);
    }
}
