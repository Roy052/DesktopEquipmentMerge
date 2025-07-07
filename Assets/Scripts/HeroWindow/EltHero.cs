using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltHero : MonoBehaviour
{
    public Text textLv;
    public Text textName;
    public Image imgHero;

    protected InfoHero infoUnit;

    public virtual void Set(InfoHero info)
    {
        infoUnit = info;
        int lv = DataLv.GetLv(info.exp);
        if (textLv)
            textLv.text = $"Lv.{lv}";
        if (textName)
            textName.text = info.strName;

        var data = DataHero.Get(info.heroId);
    }

}
