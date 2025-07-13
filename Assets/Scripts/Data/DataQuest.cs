using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class DataQuest : IRegistrable
{
    public static Dictionary<int, DataQuest> dictDataQuests = new();
    static Dictionary<TraderType, List<DataQuest>> dictDataQuestsByTrader = new();

    public int id;
    public TraderType traderType;
    public string tagName;
    public string tagDesc;
    public List<ConditionTypeValue> conditions;
    public List<ItemIdCount> requireItems;
    public List<ItemIdCount> rewardItems;
    public int rewardTraderExp;

    public void Register()
    {
        dictDataQuests.Add(id, this);

        if (dictDataQuestsByTrader.TryGetValue(traderType, out var list) == false)
            dictDataQuestsByTrader.Add(traderType, new List<DataQuest>() { this });
        else
            list.Add(this);
    }

    public static DataQuest Get(int id)
    {
        dictDataQuests.TryGetValue(id, out var dataQuest);
        return dataQuest;
    }

    public static List<DataQuest> GetAllByTrader(TraderType traderType)
    {
        dictDataQuestsByTrader.TryGetValue(traderType, out var list);
        return list;
    }

    public static List<DataQuest> GetAll()
    {
        return dictDataQuests.Values.ToList();
    }
}

[Serializable]
public class TempDataQuest : IConvertable<DataQuest>
{
    public int id;
    public TraderType traderType;
    public string tagName;
    public string tagDesc;
    public ConditionType conditionType1;
    public int condition1Value1;
    public int condition1Value2;
    public ConditionType conditionType2;
    public int condition2Value1;
    public int condition2Value2;
    public ConditionType conditionType3;
    public int condition3Value1;
    public int condition3Value2;
    public int requireItemId1;
    public int requireItemCount1;
    public int requireItemId2;
    public int requireItemCount2;
    public int requireItemId3;
    public int requireItemCount3;
    public int rewardItemId1;
    public int rewardItemCount1;
    public int rewardItemId2;
    public int rewardItemCount2;
    public int rewardItemId3;
    public int rewardItemCount3;
    public int rewardTraderExp;

    public DataQuest ConvertTo()
    {
        DataQuest temp = new DataQuest()
        {
            id = id,
            traderType = traderType,
            tagName = tagName,
            tagDesc = tagDesc,
            rewardTraderExp = rewardTraderExp
        };

        temp.conditions = new List<ConditionTypeValue>
            {
                new ConditionTypeValue()
                {
                    type = conditionType1,
                    value1 = condition1Value1,
                    value2 = condition1Value2,
                },
                new ConditionTypeValue()
                {
                    type = conditionType2,
                    value1 = condition2Value1,
                    value2 = condition2Value2,
                },
                new ConditionTypeValue()
                {
                    type = conditionType3,
                    value1 = condition3Value1,
                    value2 = condition3Value2,
                }
            };

        temp.requireItems = new List<ItemIdCount>()
            {
                new ItemIdCount()
                {
                    itemId = requireItemId1,
                    itemCount = requireItemCount1,
                },
                new ItemIdCount()
                {
                    itemId = requireItemId2,
                    itemCount = requireItemCount2,
                },
                new ItemIdCount()
                {
                    itemId = requireItemId3,
                    itemCount = requireItemCount3,
                }
            };

        temp.rewardItems = new List<ItemIdCount>()
            {
                new ItemIdCount()
                {
                    itemId = rewardItemId1,
                    itemCount = rewardItemCount1,
                },
                new ItemIdCount()
                {
                    itemId = rewardItemId2,
                    itemCount = rewardItemCount2,
                },
                new ItemIdCount()
                {
                    itemId = rewardItemId3,
                    itemCount = rewardItemCount3,
                }
            };

        return temp;
    }
}