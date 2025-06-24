using System.Collections.Generic;
using UnityEngine;

public class HeroMemberUI : WindowUI
{
    public EltHero eltHero;

    List<EltHero> eltHeroList = new List<EltHero>();

    public override void Show()
    {
        base.Show();
        var list = Singleton.gm.gameData.infoHeroes;
        for(int i = 0; i < list.Count; i++)
        {
            var elt = Utilities.GetOrCreate(eltHeroList, i, eltHero.gameObject);
            elt.Set(list[i]);
            elt.SetActive(true);
        }
        Utilities.DeactivateSurplus(eltHeroList, list.Count);
    }
}
