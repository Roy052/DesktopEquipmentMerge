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
    public Text textExpeditionCost;

    List<EltHeroExpeditionMember> eltHeroes = new List<EltHeroExpeditionMember>();
    HashSet<int> selectedMembers = new HashSet<int>();
    DataExpedition dataExpedition;
    bool isConditionClear = false;
    long currentCost = 0;

    public void Awake()
    {
        Observer.onRefreshHeroes += SetMember;
        Observer.onRefreshInjuryHero += SetMemberOne;
    }

    public void OnDestroy()
    {
        Observer.onRefreshHeroes -= SetMember;
        Observer.onRefreshInjuryHero -= SetMemberOne;
    }

    public override void Show()
    {
        base.Show();
        SetMember();
    }

    public void Set(DataExpedition data)
    {
        dataExpedition = data;

    }

    public void SetMember()
    {
        selectedMembers.Clear();
        currentCost = 0;

        var list = Singleton.gm.gameData.infoHeroes;
        if (list == null)
        {
            Debug.LogError("No Member Exist");
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].state == HeroState.InExpedition || list[i].state == HeroState.InInjury)
                continue;

            var elt = Utilities.GetOrCreate(eltHeroes, i, eltHero.gameObject);
            elt.Set(list[i], i, OnClickHero);
            elt.SetActive(true);
        }

        Utilities.DeactivateSurplus(eltHeroes, list.Count);
    }

    public void SetMemberOne(int idx)
    {
        if (eltHeroes.Count <= idx)
            return;

        eltHeroes[idx].Set(Singleton.gm.gameData.infoHeroes[idx], idx, OnClickHero);
    }

    public void OnClickHero(int idx, bool isSelect)
    {
        if (isSelect)
        {
            selectedMembers.Add(idx);
            currentCost += Singleton.gm.gameData.infoHeroes[idx].expeditionCost;
        }
        else
        {
            selectedMembers.Remove(idx);
            currentCost -= Singleton.gm.gameData.infoHeroes[idx].expeditionCost;
        }

        textExpeditionCost.text = currentCost.ToString();
        RefreshBtn();
    }

    public void OnClickGoExpedition()
    {
        if (isConditionClear == false)
            return;

        if (selectedMembers.Count == 0)
            return;

        if (Singleton.gm.gameData.IsItemEnough(DataItem.GoldId, currentCost) == false)
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

    public bool IsEmptyMember()
    {
        return selectedMembers.Count == 0;
    }
}
