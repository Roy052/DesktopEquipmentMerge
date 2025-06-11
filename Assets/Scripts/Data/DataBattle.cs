using System;
using System.Collections.Generic;

[Serializable]
public class DataBattle : IRegistrable
{
    public static Dictionary<short, DataBattle> dictDataBattles;

    public short id;
    public string tagName;
    public int itemCount;
    public int rewardProbId;
    public int rewardGoldCount;

    public void Register()
    {
        dictDataBattles.Add(id, this);
    }
}
