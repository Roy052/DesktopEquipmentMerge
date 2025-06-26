using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData
{
    public List<int> storedEquipmentList = new List<int>();

    public List<InfoHero> infoHeroes = new List<InfoHero>();
    public Dictionary<short, InfoExpedition> dictInfoExpeditions = new Dictionary<short, InfoExpedition>();
    public List<InfoQuest> infoQuests = new List<InfoQuest>();
    public Dictionary<short, long> itemCounts = new Dictionary<short, long>();
    public Dictionary<TraderType, short> traderLvs = new Dictionary<TraderType, short>();
    public List<InfoHero> infoHeroRecruits = new List<InfoHero>();
    public bool[] isHeroRecruited;
    public TimeSpan recruitRefreshRemainTime = TimeSpan.Zero;
    public HashSet<int> alreadyExpeditionHeroIds = new HashSet<int>();
    public List<short> buildingLvs = new List<short>();

    public GameData()
    {
        isHeroRecruited = new bool[3];

        for (int i = 0; i < (int)BuildingType.Max; i++)
            buildingLvs.Add(0);
    }

    public void RefreshExpedition()
    {
        foreach(var (infoId, info) in dictInfoExpeditions)
        {
            DataExpedition data = DataExpedition.Get(infoId);
            if (info.state == ExpeditionState.Progress && DateTime.Now >= info.startTime.AddMinutes(data.expeditionTime))
                info.state = ExpeditionState.Reward;
        }
        Observer.onRefreshExpedition?.Invoke();

        //dictInfoExpeditions = dictInfoExpeditions
        //    .Where(kv => kv.Value.state != ExpeditionState.End)
        //    .ToDictionary(kv => kv.Key, kv => kv.Value);
    }

    public void RefreshAlreadyExpeditionHero()
    {
        alreadyExpeditionHeroIds.Clear();

        foreach(var (infoId, info) in dictInfoExpeditions)
        {
            foreach(var id in info.heroIdxes)
            {
                if (alreadyExpeditionHeroIds.Add(id) == false)
                    Debug.Log($"Already Exist Hero Id");
            }
        }
    }

    public void AddItems(List<(short, long)> items)
    {
        foreach(var (itemId, itemCount) in items)
        {
            if (itemId == 1)
                storedEquipmentList.Add(itemId);

            if (itemCounts.ContainsKey(itemId))
                itemCounts[itemId] += itemCount;
            else
                itemCounts[itemId] = itemCount;
        }
    }

    public void AddInfoUnit(InfoHero infoHero)
    {
        infoHeroes.Add(infoHero);
    }

    public void AddInfoExpedition(InfoExpedition infoExpedition)
    {
        dictInfoExpeditions.Add(infoExpedition.expeditionId, infoExpedition);
        RefreshAlreadyExpeditionHero();
    }

    public void RecruitUnit(int idx)
    {
        InfoHero info = infoHeroRecruits[idx];
        itemCounts.TryGetValue(0, out long currentItemCount);
        if (currentItemCount < info.price)
            return;

        itemCounts[0] -= info.price;
        infoHeroes.Add(info);
        isHeroRecruited[idx] = true;
        Observer.onRefreshRecruit?.Invoke();
    }

    public void RefreshRecruitList()
    {
        infoHeroRecruits.Clear();
        if (isHeroRecruited == null || isHeroRecruited.Length < 3)
            isHeroRecruited = new bool[3];

        recruitRefreshRemainTime = TimeSpan.FromMinutes(5f);
        for(int i = 0; i < 3; i++)
        {
            InfoHero info = new InfoHero()
            {
                heroId = (short)UnityEngine.Random.Range(0, (int)RoleType.Max),
                exp = 0,
                strName = $"Do Re {UnityEngine.Random.Range(0, 100)}",
                weaponId = -1,
                armorId = -1,
                price = UnityEngine.Random.Range(10, 15)
            };
            infoHeroRecruits.Add(info);
            isHeroRecruited[i] = false;
        }
    }

    public InfoExpedition GetCurrentExpedition(short expeditionId)
    {
        dictInfoExpeditions.TryGetValue(expeditionId, out var info);
        return info;
    }
}
