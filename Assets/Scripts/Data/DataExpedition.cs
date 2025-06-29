using System;
using System.Collections.Generic;
using System.Linq;

public enum HeroConditionType
{
    None = -1,
    Role = 0,
    Lv,
}

public class HeroConditionTypeValue
{
    public HeroConditionType type;
    public int value1;
    public int value2;
}

[Serializable]
public class DataExpedition : IRegistrable
{
    public static Dictionary<short, DataExpedition> dictDataExpeditions = new Dictionary<short, DataExpedition>();

    public short id;
    public string tagName;
    public string resImage;
    public int expeditionTime;
    public int equipmentCount;
    public int rewardProbId;
    public List<HeroConditionTypeValue> conditions;

    public void Register()
    {
        dictDataExpeditions.Add(id, this);
    }

    public static DataExpedition Get(short id)
    {
        dictDataExpeditions.TryGetValue(id, out var data);
        return data;
    }

    public static List<DataExpedition> GetAll()
    {
        return dictDataExpeditions.Values.ToList();
    }
}

[Serializable]
public class TempDataExpedition : IConvertable<DataExpedition>
{
    public short id;
    public string tagName;
    public string resImage;
    public int expeditionTime;
    public int equipmentCount;
    public int rewardProbId;
    public HeroConditionType conditionType1;
    public int condition1Value1;
    public int condition1Value2;
    public HeroConditionType conditionType2;
    public int condition2Value1;
    public int condition2Value2;
    public HeroConditionType conditionType3;
    public int condition3Value1;
    public int condition3Value2;

    public DataExpedition ConvertTo()
    {
        DataExpedition temp = new DataExpedition()
        {
            id = id,
            tagName = tagName,
            resImage = resImage,
            expeditionTime = expeditionTime,
            equipmentCount = equipmentCount,
            rewardProbId = rewardProbId,
        };

        temp.conditions = new List<HeroConditionTypeValue>
        {
            new HeroConditionTypeValue()
            {
                type = conditionType1,
                value1 = condition1Value1,
                value2 = condition1Value2,
            },
            new HeroConditionTypeValue()
            {
                type = conditionType2,
                value1 = condition2Value1,
                value2 = condition2Value2,
            },
            new HeroConditionTypeValue()
            {
                type = conditionType3,
                value1 = condition3Value1,
                value2 = condition3Value2,
            }
        };

        return temp;
    }
}
