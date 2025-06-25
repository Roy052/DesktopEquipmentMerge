using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExpeditionMemberUI : WindowUI
{
    public EltHeroExpeditionMember eltHero;

    List<EltHeroExpeditionMember> eltHeroes = new List<EltHeroExpeditionMember>();
    HashSet<int> selectedMembers = new HashSet<int>();
    DataExpedition dataExpedition;

    public override void Show()
    {
        base.Show();
        var list = Singleton.gm.gameData.infoHeroes;
        if (list == null)
        {
            Debug.LogError("No Member Exist");
            return;
        }

        for(int i = 0; i < list.Count; i++)
        {
            var elt = Utilities.GetOrCreate(eltHeroes, i, eltHero.gameObject);
            elt.Set(list[i], i, OnClickHero);
            elt.SetActive(true);
        }

        Utilities.DeactivateSurplus(eltHeroes, list.Count);
    }

    public void Set(DataExpedition data)
    {
        dataExpedition = data;

    }

    public void OnClickHero(int idx, bool isSelect)
    {
        if (isSelect)
            selectedMembers.Add(idx);
        else
            selectedMembers.Remove(idx);
    }

    public void OnClickGoExpedition()
    {
        InfoExpedition info = new InfoExpedition()
        {
            expeditionId = dataExpedition.id,
            startTime = DateTime.Now,
            state = ExpeditionState.Progress,
            heroIdxes = selectedMembers.ToList(),
        };
        Singleton.gm.gameData.AddInfoExpedition(info);
        Hide();
    }
}
