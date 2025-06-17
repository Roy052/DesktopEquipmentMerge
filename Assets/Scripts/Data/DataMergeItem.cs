using System;
using System.Collections.Generic;

[Serializable]
public class DataMergeItem : IRegistrable
{
    public static Dictionary<short, DataMergeItem> dictDataMergeItems = new Dictionary<short, DataMergeItem>();

    public short id;
    public int price;

    public void Register()
    {
        dictDataMergeItems.Add(id, this);
    }
}
