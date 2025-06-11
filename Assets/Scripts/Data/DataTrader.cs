using System;
using System.Collections.Generic;

[Serializable]
public class DataTrader : IRegistrable
{
    public static Dictionary<short, DataTrader> dictDataTraders;

    public short id;
    public string tagName;
    public string resImage;
    public int missionId;

    public void Register()
    {
        dictDataTraders.Add(id, this);
    }
}