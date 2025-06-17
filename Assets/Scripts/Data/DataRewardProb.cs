using System;
using System.Collections.Generic;

[Serializable]
public class DataRewardProb : IRegistrable
{
    public static Dictionary<short, DataRewardProb> dictDataRewardProbs = new Dictionary<short, DataRewardProb>();

    public short id;
    public List<int> probs;

    public void Register()
    {
        dictDataRewardProbs.Add(id, this);
    }
}

[Serializable]
public class TempDataRewardProb : IConvertable<DataRewardProb>
{
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

    public DataRewardProb ConvertTo()
    {
        DataRewardProb temp = new DataRewardProb()
        {
            id = id,
            probs = new List<int>()
            {
                prob1,
                prob2,
                prob3,
                prob4,
                prob5,
                prob6,
                prob7,
                prob8,
                prob9,
                prob10,
                prob11,
                prob12,
            }
        };

        return temp;
    }
}