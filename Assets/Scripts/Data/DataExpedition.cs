using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class DataExpedition : IRegistrable
{
    public static Dictionary<short, DataExpedition> dictDataBattles;

    public short id;
    public string tagName;
    public int itemCount;
    public int rewardProbId;
    public int rewardGoldCount;

    public void Register()
    {
        dictDataBattles.Add(id, this);
    }

    public static List<DataExpedition> GetAll()
    {
        return dictDataBattles.Values.ToList();
    }
}
