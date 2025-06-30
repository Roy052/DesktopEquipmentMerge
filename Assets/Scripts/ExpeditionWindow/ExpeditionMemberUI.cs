using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionMemberUI : WindowUI
{
    public EltHeroExpeditionMember eltHero;
    public Image imgGoExpedition;
    public Button btnGoExpedition;

    List<EltHeroExpeditionMember> eltHeroes = new List<EltHeroExpeditionMember>();
    HashSet<int> selectedMembers = new HashSet<int>();
    DataExpedition dataExpedition;
    bool isConditionClear = false;

    public override void Show()
    {
        base.Show();
        
    }

    public void Set(DataExpedition data)
    {
        dataExpedition = data;

    }

    public void SetMember()
    {
        var list = Singleton.gm.gameData.infoHeroes;
        var alreadyList = Singleton.gm.gameData.alreadyExpeditionHeroIdxs;
        if (list == null)
        {
            Debug.LogError("No Member Exist");
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (alreadyList.Contains(i))
                continue;

            var elt = Utilities.GetOrCreate(eltHeroes, i, eltHero.gameObject);
            elt.Set(list[i], i, OnClickHero);
            elt.SetActive(true);
        }

        Utilities.DeactivateSurplus(eltHeroes, list.Count);
    }

    public void OnClickHero(int idx, bool isSelect)
    {
        if (isSelect)
            selectedMembers.Add(idx);
        else
            selectedMembers.Remove(idx);

        RefreshBtn();
    }

    public void OnClickGoExpedition()
    {
        if (isConditionClear == false)
            return;

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

    void RefreshBtn()
    {
        isConditionClear = Singleton.gm.gameData.IsHeroConditionClear(dataExpedition.id, selectedMembers.ToList());
    }
}
