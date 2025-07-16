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
            elt.objInExpedition.SetActive(infoHeroes[i].state == HeroState.InExpedition);
            elt.objInInjury.SetActive(infoHeroes[i].state == HeroState.InInjury);
            elt.SetActive(true);
        }
        Utilities.DeactivateSurplus(eltHeroMemberList, infoHeroes.Count);
    }

    public override void Hide()
    {
        base.Hide();
        heroMemberInfoUI.Hide();
    }

    public void OnClickMember(int idx)
    {
        heroMemberInfoUI.Set(infoHeroes[idx]);
        heroMemberInfoUI.Show();
    }
}
