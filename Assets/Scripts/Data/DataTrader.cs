using System;
using System.Collections.Generic;

[Serializable]
public class DataTrader : IRegistrable
{
    public static Dictionary<short, DataTrader> dictDataTraders = new Dictionary<short, DataTrader>();

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

    public void Register()
    {
        dictDataTraders.Add(id, this);
    }
}