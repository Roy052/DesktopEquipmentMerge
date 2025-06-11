using System;
using System.Collections.Generic;

[Serializable]
public class DataRewardProb : IRegistrable
{
    public static Dictionary<short, DataRewardProb> dictDataRewardProbs;

    public short id;
    public int[] prob;

    public void Register()
    {
        dictDataRewardProbs.Add(id, this);
    }
}
