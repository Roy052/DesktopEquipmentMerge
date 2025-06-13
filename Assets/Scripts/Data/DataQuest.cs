using System;
using System.Collections.Generic;

[Serializable]
public class DataQuest : IRegistrable
{
    public static Dictionary<short, DataQuest> dictDataQuests = new Dictionary<short, DataQuest>();

    public short id;
    public int traderId;
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

    public void Register()
    {
        dictDataQuests.Add(id, this);
    }
}