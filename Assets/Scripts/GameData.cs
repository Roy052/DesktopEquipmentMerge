using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class GameData
{
    public Dictionary<MergeItemCategory, List<int>> storedEquipmentList = new();

    public int[,] mergeItems;
    public List<InfoHero> infoHeroes = new();
    public Dictionary<short, InfoExpedition> dictInfoExpeditions = new();
    public Dictionary<TraderType, List<InfoQuest>> traderQuests = new();
    public Dictionary<int, long> itemCounts = new();
    public Dictionary<TraderType, short> traderLvs = new();
    public List<InfoHero> infoHeroRecruits = new();
    public bool[] isHeroRecruited;
    public TimeSpan recruitRefreshRemainTime = TimeSpan.Zero;
    public HashSet<int> alreadyExpeditionHeroIdxs = new HashSet<int>();
    public short[] buildingLvs;

    public GameData()
    {
        mergeItems = new int[3, 3];
        isHeroRecruited = new bool[3];
        buildingLvs = new short[(int)BuildingType.Max];
        
        //Set Quests
        for(TraderType type = TraderType.None + 1; type < TraderType.Max; type++)
        {
            HashSet<int> checkId = new HashSet<int>();
            if (traderQuests.TryGetValue(type, out var list) == false)
            {
                list = new List<InfoQuest>();
                traderQuests.Add(type, list);
            }
            else
            {
                foreach(var item in list)
                    checkId.Add(item.questId);
            }

            var dataQuestList = DataQuest.GetAllByTrader(type);
            foreach (var dataQuest in dataQuestList)
            {
                if (checkId.Contains(dataQuest.id))
                    continue;

                InfoQuest temp = new()
                {
                    state = IsConditionClear(dataQuest.conditions) ? QuestState.NotAccept : QuestState.NotOpened,
                    questId = dataQuest.id,
                    questProgress = new List<int>()
                };
                foreach (var require in dataQuest.requireItems)
                {
                    if (require.itemId != -1)
                        temp.questProgress.Add(0);
                }
                list.Add(temp);

                checkId.Add(dataQuest.id);
            }
        }


        traderLvs.Add(TraderType.DrakoKingdom, 1);
    }

    public void RefreshExpedition()
    {
        List<short> removeList = new List<short>();
        foreach(var (infoId, info) in dictInfoExpeditions)
        {
            DataExpedition data = DataExpedition.Get(infoId);
            if (info.state == ExpeditionState.Progress && DateTime.Now >= info.startTime.AddMinutes(data.expeditionTime))
                info.state = ExpeditionState.Reward;

            if (info.state == ExpeditionState.End)
                removeList.Add(infoId);
        }

        while (removeList.Count > 0)
        {
            dictInfoExpeditions.Remove(removeList[removeList.Count - 1]);
            removeList.RemoveAt(removeList.Count - 1);
        }
        Observer.onRefreshExpedition?.Invoke();
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

    public void AddItems(List<(int, long)> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            int itemId = item.Item1;
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
                            storedEquipmentList[category] = new List<int> { itemId };
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
                            storedEquipmentList[category] = new List<int> { itemId };
                        else
                            list.Add(itemId);
                    }
                }
                else
                {
                    if (storedEquipmentList.TryGetValue(dataMergeItem.category, out var list) == false)
                        storedEquipmentList[dataMergeItem.category] = new List<int> { itemId };
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

        DataRewardProb dataProb = DataRewardProb.Get(dataExpedition.rewardProbId);
        if(dataProb == null)
        {
            Debug.LogError($"DataRewardProb {dataExpedition.rewardProbId} Not Exist");
            return;
        }

        Dictionary<int, long> dicItems = new Dictionary<int, long>();
        for (int i = 0; i < dataExpedition.equipmentCount; i++)
        {
            MergeItemType randomType = dataProb.types[UnityEngine.Random.Range(0, dataProb.types.Count)];

            //
            int totalWeight = 0;
            for (int j = 0; j < dataProb.probs.Count; j++)
                totalWeight += dataProb.probs[j];

            if(totalWeight == 0)
            {
                Debug.LogError($"Total Weight is 0");
                return;
            }

            int pickValue = UnityEngine.Random.Range(0, totalWeight);
            int acc = 0;
            short randomGrade = 0;
            for(int j = 0; j < dataProb.probs.Count; j++)
            {
                if (dataProb.probs[j] == 0)
                    continue;

                acc += dataProb.probs[j];
                if(pickValue < acc)
                {
                    randomGrade = (short)(j + 1);
                    break;
                }
            }

            int key = DataMergeItem.GetMergeItemId(randomType, randomGrade);
            if (dicItems.ContainsKey(key))
                dicItems[key] += 1;
            else
                dicItems.Add(key, 1);
        }

        List<(int, long)> items = new List<(int, long)>();
        foreach (var kvp in dicItems)
            items.Add((kvp.Key, kvp.Value));

        AddItems(items);
        
        Observer.onRefreshExpedition?.Invoke();
        Observer.onRefreshChest?.Invoke();
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
        DataQuest dataQuest = DataQuest.Get(questId);
        if (dataQuest == null)
            return true;

        var questInfo = traderQuests[dataQuest.traderType].Find(x => x.questId == questId);
        if (questInfo != null && questInfo.state == QuestState.Clear)
            return true;

        return false;
    }

    public bool IsConditionClear(List<ConditionTypeValue> conditions)
    {
        if (conditions == null) return false;

        foreach(var condition in conditions)
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

    public int GetCountMergeItemId(int itemId)
    {
        int count = 0;
        int row = mergeItems.GetLength(0);
        int col = mergeItems.GetLength(1);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (itemId == mergeItems[i, j])
                    count++;
            }
        }

        return count;
    }

    public void OnRemoveMergeItem(int itemId, int count)
    {
        if (count == 0)
            return;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (itemId == mergeItems[i,j])
                {
                    mergeItems[i, j] = 0;
                    count -= 1;
                }

                if (count <= 0)
                    break;
            }
        }

        Observer.onRefreshMergeWindow?.Invoke();
    }

    public void OnMergeItems((int, int) coord1, (int, int) coord2)
    {
        int id1 = mergeItems[coord1.Item1, coord1.Item2];
        int id2 = mergeItems[coord2.Item1, coord2.Item2];

        if (id1 == 0 || id1 != id2)
            return;

        if (DataMergeItem.IsMaxGrade(id2))
            return;

        int nextId = DataMergeItem.GetNextGrade(id2)?.id ?? 0;
        mergeItems[coord1.Item1, coord1.Item2] = 0;
        mergeItems[coord2.Item1, coord2.Item2] = nextId;

        Observer.onRefreshMergeWindow?.Invoke();
    }

    public void OnChestToAddMergeItem(MergeItemCategory category)
    {
        if (storedEquipmentList.ContainsKey(category) == false || storedEquipmentList[category].Count == 0)
        {
            Debug.Log("Not Exist Stored Equipment");
            return;
        }

        List<(int, int)> emptyCoord = new List<(int, int)>();
        int xLength = mergeItems.GetLength(0);
        int yLength = mergeItems.GetLength(1);
        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < yLength; j++)
            {
                if (mergeItems[i, j] == 0)
                    emptyCoord.Add((i, j));
            }
        }

        var randomCoord = emptyCoord[UnityEngine.Random.Range(0, emptyCoord.Count)];
        int id = Singleton.gm.gameData.storedEquipmentList[category][0];
        Singleton.gm.gameData.storedEquipmentList[category].RemoveAt(0);
        mergeItems[randomCoord.Item1, randomCoord.Item2] = id;

        Observer.onRefreshChest?.Invoke();
        Observer.onRefreshMergeWindow?.Invoke();
    }

    public void OnRewardQuest(InfoQuest info)
    {
        if (info.state != QuestState.Reward)
            return;

        DataQuest dataQuest = DataQuest.Get(info.questId);
        List<(int, long)> rewards = new();
        foreach(var reward in dataQuest.rewardItems)
            rewards.Add((reward.itemId, reward.itemCount));
        AddItems(rewards);

        info.state = QuestState.Clear;
    }
}