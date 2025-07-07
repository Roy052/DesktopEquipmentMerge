using System;
using System.Collections.Generic;

[Serializable]
public class DataLv : IRegistrable
{
    public static short MaxLv = 0;

    public static Dictionary<short, DataLv> dictLvs = new Dictionary<short, DataLv>();

    public short lv;
    public int expMin;
    public int expMax;
    public int traderExpMin;
    public int traderExpMax;

    public void Register()
    {
        dictLvs.Add(lv, this);
        if (lv > MaxLv)
            MaxLv = lv;
    }

    public static int GetLv(int exp)
    {
        foreach(var kvp in dictLvs)
        {
            if (exp >= kvp.Value.expMin && exp <= kvp.Value.expMax)
                return kvp.Key;
        }

        return 1;
    }

    public static int GetTraderLv(int exp)
    {
        foreach (var kvp in dictLvs)
        {
            if (exp >= kvp.Value.traderExpMin && exp <= kvp.Value.traderExpMax)
                return kvp.Key;
        }

        return 1;
    }
}
