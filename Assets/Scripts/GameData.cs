using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public List<int> storedEquipmentList = new List<int>();

    public List<InfoHero> infoHeroes = new List<InfoHero>();
    public List<InfoQuest> infoQuests = new List<InfoQuest>();
    public Dictionary<short, long> itemCounts = new Dictionary<short, long>();
    public Dictionary<TraderType, short> traderLvs = new Dictionary<TraderType, short>();
    public List<InfoHero> infoHeroRecruits = new List<InfoHero>();
    public TimeSpan remainTime = TimeSpan.Zero;

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

    public void RecruitUnit(int idx)
    {
        InfoHero info = infoHeroRecruits[idx];
        itemCounts.TryGetValue(0, out long currentItemCount);
        if (currentItemCount < info.price)
            return;

        infoHeroes.Add(info);
    }

    public void AddInfoUnit(InfoHero infoHero)
    {
        infoHeroes.Add(infoHero);
    }

    public void RefreshRecruitList()
    {
        infoHeroRecruits.Clear();
        remainTime = TimeSpan.FromMinutes(5f);
        for(int i = 0; i < 3; i++)
        {
            InfoHero info = new InfoHero()
            {
                heroId = (short)UnityEngine.Random.Range(0, (int)RoleType.Max),
                exp = 0,
                strName = "Do Re",
                weaponId = -1,
                armorId = -1,
                price = UnityEngine.Random.Range(10, 15)
            };
            infoHeroRecruits.Add(info);
        }
    }
}
