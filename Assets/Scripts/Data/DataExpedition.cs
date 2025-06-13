using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class DataExpedition : IRegistrable
{
    public static Dictionary<short, DataExpedition> dictDataExpeditions = new Dictionary<short, DataExpedition>();

    public short id;
    public string tagName;
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
    public int equipmentCount;
    public int rewardProbId;

    public void Register()
    {
        dictDataExpeditions.Add(id, this);
    }

    public static List<DataExpedition> GetAll()
    {
        return dictDataExpeditions.Values.ToList();
    }
}
