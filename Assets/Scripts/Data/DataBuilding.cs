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
    public static Dictionary<BuildingType, DataBuilding> dictDataBuildings;

    public BuildingType type;
    public string tagName;
    public BuildingConditionType conditionType1;
    public int condition1Value1;
    public int condition1Value2;
    public BuildingConditionType conditionType2;
    public int condition2Value1;
    public int condition2Value2;
    public BuildingConditionType conditionType3;
    public int condition3Value1;
    public int condition3Value2;
    public int goldCount;

    public void Register()
    {
        dictDataBuildings.Add(type, this);
    }
}

public enum BuildingConditionType
{
    ItemCount,
    TraderLv,
    MissionClear,
}

public class BuildingCondition
{
    public BuildingConditionType conditionType1;
    public int value1;
    public int value2;
}

public class TempDataBuilding
{
    public BuildingType type;
    public string tagName;
    public BuildingConditionType conditionType1;
    public int condition1Value1;
    public int condition1Value2;
    public BuildingConditionType conditionType2;
    public int condition2Value1;
    public int condition2Value2;
    public BuildingConditionType conditionType3;
    public int condition3Value1;
    public int condition3Value2;
    public int goldCount;


}