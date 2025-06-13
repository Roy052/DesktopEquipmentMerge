using System;
using System.Collections.Generic;

[Serializable]
public class DataRewardProb : IRegistrable
{
    public static Dictionary<short, DataRewardProb> dictDataRewardProbs = new Dictionary<short, DataRewardProb>();

    public short id;
    public int prob1;
    public int prob2;
    public int prob3;
    public int prob4;
    public int prob5;
    public int prob6;
    public int prob7;
    public int prob8;
    public int prob9;
    public int prob10;
    public int prob11;
    public int prob12;

    public void Register()
    {
        dictDataRewardProbs.Add(id, this);
    }
}
