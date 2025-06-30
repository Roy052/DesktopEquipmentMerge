using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData
{
    public Dictionary<MergeItemCategory, List<short>> storedEquipmentList = new();

    public List<InfoHero> infoHeroes = new();
    public Dictionary<short, InfoExpedition> dictInfoExpeditions = new();
    public List<InfoQuest> infoQuests = new();
    public Dictionary<short, long> itemCounts = new();
    public Dictionary<TraderType, short> traderLvs = new();
    public List<InfoHero> infoHeroRecruits = new();
    public bool[] isHeroRecruited;
    public TimeSpan recruitRefreshRemainTime = TimeSpan.Zero;
    public HashSet<int> alreadyExpeditionHeroIdxs = new HashSet<int>();
    public short[] buildingLvs;

    public GameData()
    {
        isHeroRecruited = new bool[3];
        buildingLvs = new short[(int)BuildingType.Max];
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
        alreadyExpeditionHeroIdxs.Clear();

        foreach(var (infoId, info) in dictInfoExpeditions)
        {
            if (info.state == ExpeditionState.End)
                continue;

            foreach(var id in info.heroIdxes)
            {
                if (alreadyExpeditionHeroIdxs.Add(id) == false)
                    Debug.Log($"Already Exist Hero Id");

                alreadyExpeditionHeroIdxs.Add(id);
            }
        }
    }

    public void AddItems(List<(short, long)> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            short itemId = item.Item1;
            long itemCount = item.Item2;

            DataItem dataItem = DataItem.Get(itemId);
            if (dataItem == null)
                continue;

            if(dataItem.type == ItemType.MergeItem)
            {
                DataMergeItem dataMergeItem = DataMergeItem.Get(dataItem.id);
                if (dataMergeItem == null)
                    continue;

                if(dataMergeItem.category == MergeItemCategory.All)
                {
                    //1. If First, Check Next
                    if(i == 0)
                    {
                        //Find Next Category
                        MergeItemCategory category = MergeItemCategory.WeaponWarrior;
                        for(int j = 1; j < items.Count; j++)
                        {
                            DataItem tempDataItem = DataItem.Get(items[j].Item1);
                            if (tempDataItem == null || tempDataItem.type != ItemType.MergeItem)
                                continue;

                            DataMergeItem tempDataMergeItem = DataMergeItem.Get(tempDataItem.id);
                            if (tempDataMergeItem == null)
                                continue;

                            category = tempDataMergeItem.category;
                            break;
                        }

                        if (storedEquipmentList.TryGetValue(category, out var list) == false)
                            storedEquipmentList[category] = new List<short> { itemId };
                        else
                            list.Add(itemId);
                    }
                    //2. If Not First, Check Before
                    else
                    {
                        MergeItemCategory category = MergeItemCategory.WeaponWarrior;
                        for (int j = 0; j < i; j++)
                        {
                            DataItem tempDataItem = DataItem.Get(items[j].Item1);
                            if (tempDataItem == null || tempDataItem.type != ItemType.MergeItem)
                                continue;

                            DataMergeItem tempDataMergeItem = DataMergeItem.Get(tempDataItem.id);
                            if (tempDataMergeItem == null)
                                continue;

                            category = tempDataMergeItem.category;
                            break;
                        }

                        if (storedEquipmentList.TryGetValue(category, out var list) == false)
                            storedEquipmentList[category] = new List<short> { itemId };
                        else
                            list.Add(itemId);
                    }
                }
                else
                {
                    if (storedEquipmentList.TryGetValue(dataMergeItem.category, out var list) == false)
                        storedEquipmentList[dataMergeItem.category] = new List<short> { itemId };
                    else
                        list.Add(itemId);
                }
            }
            else
            {
                if (itemCounts.ContainsKey(itemId))
                    itemCounts[itemId] += itemCount;
                else
                    itemCounts[itemId] = itemCount;
            }
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

    public void GetRewardExpedition(short expeditionId)
    {
        var info = GetCurrentExpedition(expeditionId);
        if (info == null || info.state != ExpeditionState.Reward)
            return;

        info.state = ExpeditionState.End;

        DataExpedition dataExpedition = DataExpedition.Get(expeditionId);
        if (dataExpedition == null)
        {
            Debug.LogError($"DataExpedition {expeditionId} Not Exist");
            return;
        }

        Dictionary<short, long> dicItems = new Dictionary<short, long>();
        for (int i = 0; i < dataExpedition.equipmentCount; i++)
        {
            short key = (short)UnityEngine.Random.Range(1, 3);
            if (dicItems.ContainsKey(key))
                dicItems[key] += 1;
            else
                dicItems.Add(key, 1);
        }

        List<(short, long)> items = new List<(short, long)>();
        foreach (var kvp in dicItems)
            items.Add((kvp.Key, kvp.Value));

        Singleton.gm.gameData.AddItems(items);
        Observer.onRefreshMergeItem?.Invoke();
    }

    public InfoExpedition GetCurrentExpedition(short expeditionId)
    {
        dictInfoExpeditions.TryGetValue(expeditionId, out var info);
        return info;
    }

    public void BuildBuilding(BuildingType buildingType)
    {
        buildingLvs[(int)buildingType] = 1;
        Observer.onRefreshBuilding?.Invoke();
    }

    public bool IsMissionClear(int questId)
    {
        var questInfo = infoQuests.Find(x => x.questId == questId);
        if (questInfo != null && questInfo.state == QuestState.End)
            return true;

        return false;
    }

    public bool IsConditionClear(int questId)
    {
        DataQuest data = DataQuest.Get(questId);
        if (data == null) return false;

        foreach(var condition in data.conditions)
        {
            switch (condition.type)
            {
                case ConditionType.None:
                    break;
                case ConditionType.ItemCount:
                    itemCounts.TryGetValue((short)condition.value1, out long itemCount);
                    if (itemCount < condition.value2)
                        return false;
                    break;
                case ConditionType.TraderLv:
                    traderLvs.TryGetValue((TraderType)condition.value1, out short lv);
                    if (lv < condition.value2)
                        return false;
                    break;
                case ConditionType.MissionClear:
                    if (IsMissionClear(condition.value1) == false)
                        return false;
                    break;
            }
        }

        return true;
    }

    public bool IsHeroConditionClear(short expeditionId, List<int> heroIdxs)
    {
        DataExpedition dataExpedition = DataExpedition.Get(expeditionId);
        if (dataExpedition == null) return false;

        foreach (var condition in dataExpedition.heroConditions)
        {
            switch (condition.type)
            {
                case HeroConditionType.None:
                    break;
                case HeroConditionType.Role:
                    int roleCount = 0;
                    for (int i = 0; i < heroIdxs.Count; i++)
                    {
                        if (infoHeroes.Count <= i)
                            break;

                        var dataHero = DataHero.Get(infoHeroes[heroIdxs[i]].heroId);
                        if (dataHero.role == (RoleType)condition.value1)
                            roleCount++;
                    }
                    if (roleCount < condition.value2)
                        return false;
                    break;
                case HeroConditionType.Lv:
                    int lvCount = 0;
                    for (int i = 0; i < heroIdxs.Count; i++)
                    {
                        if (infoHeroes.Count <= i)
                            break;

                        if (DataLv.GetLv(infoHeroes[i].exp) >= condition.value1)
                            lvCount++;
                    }
                    if (lvCount < condition.value2)
                        return false;
                    break;
            }
        }

        return true;
    }
}