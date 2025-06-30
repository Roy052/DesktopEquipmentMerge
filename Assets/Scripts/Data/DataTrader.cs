using System;
using System.Collections.Generic;

public enum TraderType
{
    None = -1,
    DrakoKingdom
}

[Serializable]
public class DataTrader : IRegistrable
{
    public static Dictionary<TraderType, List<DataTrader>> dictDataTraders = new Dictionary<TraderType, List<DataTrader>>();

    public TraderType type;
    public short lv;
    public string tagName;
    public string resImage;
    public List<ConditionTypeValue> conditions;

    public void Register()
    {
        if (dictDataTraders.TryGetValue(type, out var list) == false)
            dictDataTraders.Add(type, new List<DataTrader>() { this });
        else
            list.Add(this);
    }

    public static DataTrader Get(TraderType type, short lv)
    {
        if (dictDataTraders.TryGetValue(type, out var list) == false)
            return null;

        return list.Find(x => x.lv == lv);
    }
}

[Serializable]
public class TempDataTrader : IConvertable<DataTrader>
{
    public TraderType type;
    public short lv;
    public string tagName;
    public string tagDesc;
    public string resImage;
    public ConditionType conditionType1;
    public int condition1Value1;
    public int condition1Value2;
    public ConditionType conditionType2;
    public int condition2Value1;
    public int condition2Value2;
    public ConditionType conditionType3;
    public int condition3Value1;
    public int condition3Value2;

    public DataTrader ConvertTo()
    {
        DataTrader temp = new DataTrader()
        {
            type = type,
            lv = lv,
            tagName = tagName,
            resImage = resImage,
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

        return temp;
    }
}