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
    public int buildingLv;
    public int missionConditionId;
    public int exp;
    public int recommendLv;
    public List<HeroConditionTypeValue> heroConditions;

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
    public int buildingLv;
    public int missionConditionId;
    public int exp;
    public int recommendLv;
    public HeroConditionType heroConditionType1;
    public int heroCondition1Value1;
    public int heroCondition1Value2;
    public HeroConditionType heroConditionType2;
    public int heroCondition2Value1;
    public int heroCondition2Value2;
    public HeroConditionType heroConditionType3;
    public int heroCondition3Value1;
    public int heroCondition3Value2;

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
            buildingLv = buildingLv,
            missionConditionId = missionConditionId,
            exp = exp,
            recommendLv = recommendLv,
        };

        temp.heroConditions = new List<HeroConditionTypeValue>
        {
            new HeroConditionTypeValue()
            {
                type = heroConditionType1,
                value1 = heroCondition1Value1,
                value2 = heroCondition1Value2,
            },
            new HeroConditionTypeValue()
            {
                type = heroConditionType2,
                value1 = heroCondition2Value1,
                value2 = heroCondition2Value2,
            },
            new HeroConditionTypeValue()
            {
                type = heroConditionType3,
                value1 = heroCondition3Value1,
                value2 = heroCondition3Value2,
            }
        };

        return temp;
    }
}
