using System;
using System.Collections.Generic;

[Serializable]
public class DataRewardProb : IRegistrable
{
    public static Dictionary<int, DataRewardProb> dictDataRewardProbs = new Dictionary<int, DataRewardProb>();

    public int id;
    public List<MergeItemType> types;
    public List<int> probs;

    public void Register()
    {
        dictDataRewardProbs.Add(id, this);
    }

    public static DataRewardProb Get(int id)
    {
        dictDataRewardProbs.TryGetValue(id, out var value);
        return value;
    }
}

[Serializable]
public class TempDataRewardProb : IConvertable<DataRewardProb>
{
    public short id;
    public MergeItemType type1;
    public MergeItemType type2;
    public MergeItemType type3;
    public MergeItemType type4;
    public MergeItemType type5;
    public MergeItemType type6;
    public MergeItemType type7;
    public MergeItemType type8;
    public MergeItemType type9;
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
            types = new List<MergeItemType>() 
            {
                type1,
                type2,
                type3,
                type4,
                type5,
                type6,
                type7,
                type8,
                type9,
            },
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