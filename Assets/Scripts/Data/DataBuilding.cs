using System;
using System.Collections.Generic;

public enum BuildingType
{
    MergeTable,
    Storage,
    ExpeditionCenter,
}

[Serializable]
public class DataBuilding : IRegistrable
{
    public static Dictionary<BuildingType, List<DataBuilding>> dictDataBuildings = new Dictionary<BuildingType, List<DataBuilding>>();

    public BuildingType type;
    public int lv;
    public string tagName;
    public List<ConditionTypeValue> conditions;
    public List<ItemIdCount> requireItems;

    public void Register()
    {
        if (dictDataBuildings.TryGetValue(type, out var list) == false)
            dictDataBuildings.Add(type, new List<DataBuilding>() { this });
        else
            list.Add(this);
    }
}

public enum ConditionType
{
    None = -1,
    ItemCount = 0, //특수 아이템
    TraderLv,
    MissionClear,
}

public class ConditionTypeValue
{
    public ConditionType type;
    public int value1;
    public int value2;
}

public class ItemIdCount
{
    public int itemId;
    public int itemCount;
}

[Serializable]
public class TempDataBuilding : IConvertable<DataBuilding>
{
    public int id;
    public BuildingType type;
    public int lv;
    public string tagName;
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

    public DataBuilding ConvertTo()
    {
        DataBuilding temp = new DataBuilding()
        {
            type = type,
            lv = lv,
            tagName = tagName,
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

        return temp;
    }
}