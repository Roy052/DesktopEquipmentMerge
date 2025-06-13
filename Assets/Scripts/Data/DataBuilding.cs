using System;
using System.Collections.Generic;

public enum BuildingType
{
    MergeTable,
    Storage,
    ExpeditionCenter,
    ExpeditionLodging,
}

[Serializable]
public class DataBuilding : IRegistrable
{
    public static Dictionary<int, DataBuilding> dictDataBuildings = new Dictionary<int, DataBuilding>();

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

    public void Register()
    {
        dictDataBuildings.Add(id, this);
    }
}

public enum ConditionType
{
    ItemCount,
    TraderLv,
    MissionClear,
}

public class BuildingCondition
{
    public ConditionType conditionType1;
    public int value1;
    public int value2;
}

public class TempDataBuilding
{
    public BuildingType type;
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
    public int goldCount;


}