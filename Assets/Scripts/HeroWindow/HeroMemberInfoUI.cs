using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroMemberInfoUI : WindowUI
{
    public Text textName;
    public Text textLv;
    public RectTransform rtGauge;
    public EltItem eltExpItem;
    List<EltItem> eltExpItems = new();

    public EltItem eltWeapon;
    public EltItem eltArmor;

    public void Set(InfoHero infoHero)
    {
        textName.text = infoHero.strName;
        textLv.text = $"Lv.{DataLv.GetLv(infoHero.exp)}";
        
        for(int i = 0; i < 3; i++)
        {
            int itemId = DataMergeItem.GetByTypeGrade(MergeItemType.ExpBook, 1).id;
            var elt = Utilities.GetOrCreate(eltExpItems, i, eltExpItem.gameObject);
            elt.Set(itemId, i);
        }

        Utilities.DeactivateSurplus(eltExpItems, 3);

        eltWeapon.Set(infoHero.weaponId, 0);
        eltArmor.Set(infoHero.armorId, 0);
    }
}
