using System.Collections.Generic;
using UnityEngine;

public class HeroMemberUI : WindowUI
{
    public EltHeroMember eltHeroMember;
    public HeroMemberInfoUI heroMemberInfoUI;

    List<EltHeroMember> eltHeroMemberList = new List<EltHeroMember>();
    List<InfoHero> infoHeroes = new();

    public override void Show()
    {
        base.Show();
        infoHeroes = Singleton.gm.gameData.infoHeroes;
        for(int i = 0; i < infoHeroes.Count; i++)
        {
            var elt = Utilities.GetOrCreate(eltHeroMemberList, i, eltHeroMember.gameObject);
            elt.Set(infoHeroes[i], i);
            elt.funcClick = OnClickMember;
            elt.SetActive(true);
        }
        Utilities.DeactivateSurplus(eltHeroMemberList, infoHeroes.Count);
    }

    public void OnClickMember(int idx)
    {
        heroMemberInfoUI.Set(infoHeroes[idx]);
        heroMemberInfoUI.Show();
    }
}
