using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pair<TKey, TValue> { public TKey key; public TValue value; }

[System.Serializable]
public class PairList<TKey, TValue>
{
    public List<Pair<TKey, TValue>> items;
    public static PairList<TKey, TValue> FromDictionary(Dictionary<TKey, TValue> dict)
    {
        var list = new PairList<TKey, TValue> { items = new List<Pair<TKey, TValue>>(dict.Count) };
        foreach (var kv in dict) list.items.Add(new Pair<TKey, TValue> { key = kv.Key, value = kv.Value });
        return list;
    }

    public Dictionary<TKey, TValue> ToDictionary()
        => items.ToDictionary(p => p.key, p => p.value);
}

[System.Serializable]
public class SerializedGameData
{
    public PairList<MergeItemCategory, List<int>> storedEquipmentList;
    public int row;
    public int col;
    public List<int> mergeItems;
    public List<InfoHero> infoHeroes = new();
    public PairList<short, InfoExpedition> dictInfoExpeditions = new();
    public PairList<TraderType, List<InfoQuest>> traderQuests = new();
    public PairList<int, long> itemCounts = new();
    public PairList<TraderType, int> traderExps = new();
    public List<InfoHero> infoHeroRecruits = new();
    public bool[] isHeroRecruited;
    public long recruitRefreshRemainTimeTick;
    public List<int> alreadyExpeditionHeroIdxs = new List<int>();
    public short[] buildingLvs;

    public GameData ToGameData()
    {
        GameData data = new()
        {
            storedEquipmentList = storedEquipmentList.ToDictionary(),
            row = row,
            col = col,
            mergeItems = new int[row, col]
        };
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
                data.mergeItems[i, j] = mergeItems[i * row + j];
        data.infoHeroes = infoHeroes;
        data.dictInfoExpeditions = dictInfoExpeditions.ToDictionary();
        data.traderQuests = traderQuests.ToDictionary();
        data.itemCounts = itemCounts.ToDictionary();
        data.traderExps = traderExps.ToDictionary();
        data.infoHeroRecruits = infoHeroRecruits;
        data.isHeroRecruited = isHeroRecruited;
        data.recruitRefreshRemainTime = TimeSpan.FromTicks(recruitRefreshRemainTimeTick);
        data.alreadyExpeditionHeroIdxs = alreadyExpeditionHeroIdxs.ToHashSet();
        data.buildingLvs = buildingLvs;

        return data;
    }

    public SerializedGameData(GameData data)
    {
        storedEquipmentList = PairList<MergeItemCategory, List<int>>.FromDictionary(data.storedEquipmentList);
        row = data.row;
        col = data.col;
        List<int> mergeItems = new List<int>();
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
                mergeItems.Add(data.mergeItems[i, j]);
        infoHeroes = data.infoHeroes;
        dictInfoExpeditions = PairList<short, InfoExpedition>.FromDictionary(data.dictInfoExpeditions);
        traderQuests = PairList<TraderType, List<InfoQuest>>.FromDictionary(data.traderQuests);
        itemCounts = PairList<int, long>.FromDictionary(data.itemCounts);
        traderExps = PairList<TraderType, int>.FromDictionary(data.traderExps);
        infoHeroRecruits = data.infoHeroRecruits;
        isHeroRecruited = data.isHeroRecruited;
        recruitRefreshRemainTimeTick = data.recruitRefreshRemainTime.Ticks;
        alreadyExpeditionHeroIdxs = data.alreadyExpeditionHeroIdxs.ToList();
        buildingLvs = data.buildingLvs;

}
}

public class GameData
{
    public Dictionary<MergeItemCategory, List<int>> storedEquipmentList = new();
    public int row;
    public int col;
    public int[,] mergeItems;
    public List<InfoHero> infoHeroes = new();
    public Dictionary<short, InfoExpedition> dictInfoExpeditions = new();
    public Dictionary<TraderType, List<InfoQuest>> traderQuests = new();
    public Dictionary<int, long> itemCounts = new();
    public Dictionary<TraderType, int> traderExps = new();
    public List<InfoHero> infoHeroRecruits = new();
    public bool[] isHeroRecruited;
    public TimeSpan recruitRefreshRemainTime = TimeSpan.Zero;
    public HashSet<int> alreadyExpeditionHeroIdxs = new HashSet<int>();
    public short[] buildingLvs;
    public bool[] isTutorialShowed;

    public GameData()
    {
        storedEquipmentList.Add(MergeItemCategory.WeaponWarrior, new List<int>() {10001, 10001 });

        row = 3;
        col = 3;
        mergeItems = new int[row, col];
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
                mergeItems[i, j] = -1;
        isHeroRecruited = new bool[3];
        buildingLvs = new short[(int)BuildingType.Max];
        buildingLvs[(int)BuildingType.Storage] = 1;

        SetQuestInfo();

        traderExps.Add(TraderType.Keeper, 1);

        isTutorialShowed = new bool[(int)TutorialType.Max];
    }

    void SetQuestInfo()
    {
        var questList = DataQuest.GetAll();
        foreach (var dataQuest in questList)
        {
            if (traderQuests.TryGetValue(dataQuest.traderType, out var list) == false)
            {
                list = new List<InfoQuest>();
                traderQuests.Add(dataQuest.traderType, list);
            }

            InfoQuest temp = new()
            {
                state = IsConditionClear(dataQuest.conditions) ? QuestState.NotAccept : QuestState.NotOpened,
                questId = dataQuest.id,
                questProgress = new List<int>()
            };
            foreach (var require in dataQuest.requireItems)
                temp.questProgress.Add(0);
            list.Add(temp);
        }
    }

    void RefreshQuestInfo()
    {
        var questList = DataQuest.GetAll();
        foreach(var (traderType, infoQuestList) in traderQuests)
        {
            foreach(var infoQuest in infoQuestList)
            {
                if (infoQuest.state == QuestState.NotOpened)
                {
                    DataQuest dataQuest = DataQuest.Get(infoQuest.questId);
                    if(dataQuest == null)
                    {
                        Debug.LogError($"Data Quest Not Exist {infoQuest.questId}");
                        continue;
                    }
                    infoQuest.state = IsConditionClear(dataQuest.conditions) ? QuestState.NotAccept : QuestState.NotOpened;
                }
            }      
        }
    }

    public static void Save(GameData gameData)
    {
        string json = JsonUtility.ToJson(new SerializedGameData(gameData));
        string path = Application.dataPath + "/Save";
        Debug.Log(path);
        File.WriteAllText(path + "/" + "Save" + 0, json);
        Debug.Log(json);
    }

    public static GameData Load()
    {
        string path = Application.dataPath + "/Save";
        if (File.Exists(path + "/" + "Save" + 0) == false)
        {
            Debug.LogError("No File Exists");
            return null;
        }
        string data = File.ReadAllText(path + "/" + "Save" + 0);
        Debug.Log(data);
        SerializedGameData seGameData = JsonUtility.FromJson<SerializedGameData>(data);
        GameData gameData = seGameData.ToGameData();
        return gameData;
    }

    public void RefreshExpedition()
    {
        List<short> removeList = new List<short>();
        foreach(var (infoId, info) in dictInfoExpeditions)
        {
            DataExpedition data = DataExpedition.Get(infoId);
            if (info.state == ExpeditionState.Progress && DateTime.Now >= info.startTime.AddSeconds(data.expeditionTime))
                info.state = ExpeditionState.Reward;

            if (info.state == ExpeditionState.End)
                removeList.Add(infoId);
        }

        while (removeList.Count > 0)
        {
            dictInfoExpeditions.Remove(removeList[removeList.Count - 1]);
            removeList.RemoveAt(removeList.Count - 1);
        }

        RefreshAlreadyExpeditionHero();
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
                        {
                            List<int> tempList = new((int)itemCount);
                            for (int j = 0; j < itemCount; j++)
                                tempList.Add(itemId);
                            storedEquipmentList[category] = tempList;
                        }
                        else
                        {
                            for (int j = 0; j < itemCount; j++)
                                list.Add(itemId);
                        }
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
                        {
                            List<int> tempList = new((int)itemCount);
                            for (int j = 0; j < itemCount; j++)
                                tempList.Add(itemId);
                            storedEquipmentList[category] = tempList;
                        }
                        else
                        {
                            for (int j = 0; j < itemCount; j++)
                                list.Add(itemId);
                        }
                    }
                }
                else
                {
                    if (storedEquipmentList.TryGetValue(dataMergeItem.category, out var list) == false)
                    {
                        List<int> tempList = new((int)itemCount);
                        for (int j = 0; j < itemCount; j++)
                            tempList.Add(itemId);
                        storedEquipmentList[dataMergeItem.category] = tempList;
                    }
                    else
                    {
                        for (int j = 0; j < itemCount; j++)
                            list.Add(itemId);
                    }
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
                heroId = (short)UnityEngine.Random.Range(0, (int)RoleType.Archer),
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

        //Items
        DataRewardProb dataProb = DataRewardProb.Get(dataExpedition.rewardProbId);
        if(dataProb == null)
        {
            Debug.LogError($"DataRewardProb {dataExpedition.rewardProbId} Not Exist");
            return;
        }

        int totalTypeWeight = 0;
        for (int j = 0; j < dataProb.probTypes.Count; j++)
            totalTypeWeight += dataProb.probTypes[j];

        if (totalTypeWeight == 0)
        {
            Debug.LogError($"Type Total Weight is 0");
            return;
        }

        int totalGradeWeight = 0;
        for (int j = 0; j < dataProb.probGrades.Count; j++)
            totalGradeWeight += dataProb.probGrades[j];

        if (totalGradeWeight == 0)
        {
            Debug.LogError($"Grade Total Weight is 0");
            return;
        }

        Dictionary<int, long> dicItems = new Dictionary<int, long>();
        for (int i = 0; i < dataExpedition.equipmentCount; i++)
        {
            int pickValue = UnityEngine.Random.Range(0, totalTypeWeight);
            int acc = 0;
            MergeItemType randomType = dataProb.types[0];
            for (int j = 0; j < dataProb.probTypes.Count; j++)
            {
                if (dataProb.probTypes[j] == 0)
                    continue;

                acc += dataProb.probTypes[j];
                if (pickValue < acc)
                {
                    randomType = dataProb.types[j];
                    break;
                }
            }

            pickValue = UnityEngine.Random.Range(0, totalGradeWeight);
            acc = 0;
            short randomGrade = 0;
            for(int j = 0; j < dataProb.probGrades.Count; j++)
            {
                if (dataProb.probGrades[j] == 0)
                    continue;

                acc += dataProb.probGrades[j];
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
        
        //Heroes
        foreach(int idx in info.heroIdxes)
            infoHeroes[idx].exp += dataExpedition.exp;

        Observer.onRefreshExpedition?.Invoke();
        Observer.onRefreshChest?.Invoke();
        Observer.onRefreshHeroes?.Invoke();
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
                    if(condition.value1 / 10000 == 1)
                    {
                        int itemCount = GetCountMergeItemId(condition.value1);
                        if (itemCount < condition.value2)
                            return false;
                    }
                    else
                    {
                        itemCounts.TryGetValue(condition.value1, out long itemCount);
                        if (itemCount < condition.value2)
                            return false;
                    }
                    break;
                case ConditionType.TraderLv:
                    traderExps.TryGetValue((TraderType)condition.value1, out int exp);
                    int lv = DataLv.GetTraderLv(exp);
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

    public List<int> GetMergeItemListByType(MergeItemType type)
    {
        List<int> list = new List<int>();
        int row = mergeItems.GetLength(0);
        int col = mergeItems.GetLength(1);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                int itemId = mergeItems[i, j];
                var dataMergeItem = DataMergeItem.Get(itemId);
                if (dataMergeItem != null && dataMergeItem.type == type)
                    list.Add(itemId);
            }
        }
        list.Sort();
        return list;
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
                    mergeItems[i, j] = -1;
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
        mergeItems[coord1.Item1, coord1.Item2] = -1;
        mergeItems[coord2.Item1, coord2.Item2] = nextId;

        Observer.onRefreshMergeWindow?.Invoke();
        Observer.onRefreshBuilding?.Invoke();
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
                if (mergeItems[i, j] == -1)
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
        traderExps.TryGetValue(dataQuest.traderType, out int exp);
        traderExps[dataQuest.traderType] = exp + dataQuest.rewardTraderExp;

        RefreshQuestInfo();
        Observer.onRefreshQuests?.Invoke();
    }

    public void ChangeEquipment(InfoHero infoHero, bool isWeapon, int selectItemId)
    {
        int row = mergeItems.GetLength(0);
        int col = mergeItems.GetLength(1);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (selectItemId == mergeItems[i, j])
                {
                    int beforeItemId = isWeapon ? infoHero.weaponId : infoHero.armorId;
                    mergeItems[i, j] = beforeItemId;
                    if (isWeapon)
                        infoHero.weaponId = selectItemId;
                    else
                        infoHero.armorId = selectItemId;
                }
            }
        }

        Observer.onRefreshMergeWindow?.Invoke();
        Observer.onRefreshHeroes?.Invoke();
    }
}