using System;
using System.Collections.Generic;

[Serializable]
public class DataRewardProb : IRegistrable
{
    public static Dictionary<int, DataRewardProb> dictDataRewardProbs = new Dictionary<int, DataRewardProb>();

    public int id;
    public List<MergeItemType> types;
    public List<int> probTypes;
    public List<int> probGrades;

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
    public int probType1;
    public int probType2;
    public int probType3;
    public int probType4;
    public int probType5;
    public int probType6;
    public int probType7;
    public int probType8;
    public int probType9;
    public int probType10;
    public int probType11;
    public int probType12;
    public int probGrade1;
    public int probGrade2;
    public int probGrade3;
    public int probGrade4;
    public int probGrade5;
    public int probGrade6;
    public int probGrade7;
    public int probGrade8;
    public int probGrade9;
    public int probGrade10;
    public int probGrade11;
    public int probGrade12;

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
            probTypes = new()
            {
                probType1,
                probType2,
                probType3,
                probType4,
                probType5,
                probType6,
                probType7,
                probType8,
                probType9,
                probType10,
                probType11,
                probType12,
            },
            probGrades = new()
            {
                probGrade1,
                probGrade2,
                probGrade3,
                probGrade4,
                probGrade5,
                probGrade6,
                probGrade7,
                probGrade8,
                probGrade9,
                probGrade10,
                probGrade11,
                probGrade12,
            }
        };

        return temp;
    }
}