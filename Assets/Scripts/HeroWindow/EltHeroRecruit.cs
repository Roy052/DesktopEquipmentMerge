using UnityEngine;
using UnityEngine.UI;

public class EltHeroRecruit : EltHero
{
    public Text textBtnHeroPrice;
    public Image imgItem;
    public GameObject objDisable;
    int idx;

    public void Set(InfoHero info, int idx, bool isDisable)
    {
        base.Set(info);
        this.idx = idx;
        textBtnHeroPrice.text = $"°í¿ë {info.price}";
        objDisable.SetActive(isDisable);
    }

    public void OnClickRecruit()
    {
        Singleton.gm.gameData.RecruitUnit(idx);
    }
}
