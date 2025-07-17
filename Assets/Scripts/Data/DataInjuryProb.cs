using System;
using System.Collections.Generic;

[Serializable]
public class DataInjuryProb : IRegistrable
{
    static int MaxDiff = int.MinValue;
    static int MinDiff = int.MaxValue;

    public static Dictionary<int, DataInjuryProb> dictDataInjuryProbs = new Dictionary<int, DataInjuryProb>();

    public int diff;
    public List<int> injuryProbs;

    public void Register()
    {
        dictDataInjuryProbs.Add(diff, this);
        if (diff > MaxDiff)
            MaxDiff = diff;
        if (diff < MinDiff) 
            MinDiff = diff;
    }

    public static DataInjuryProb Get(int diff)
    {
        if (diff > MaxDiff)
            diff = MaxDiff;

        if (diff < MinDiff)
            diff = MinDiff;

        dictDataInjuryProbs.TryGetValue(diff, out var value);
        return value;
    }
}

[Serializable]
public class TempDataInjuryProb : IConvertable<DataInjuryProb>
{
    public int diff;
    public int noProb;
    public int minorProb;
    public int normalProb;
    public int seriousProb;

    public DataInjuryProb ConvertTo()
    {
        DataInjuryProb temp = new DataInjuryProb()
        {
            diff = diff,
            injuryProbs = new()
            {
                noProb,
                minorProb,
                normalProb,
                seriousProb
            }
        };

        return temp;
    }
}