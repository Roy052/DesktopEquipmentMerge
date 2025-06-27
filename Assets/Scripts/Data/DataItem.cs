using System;
using System.Collections.Generic;

public enum ItemType
{
    Money,
    MergeItem,
    Quest,
}

[Serializable]
public class DataItem : IRegistrable
{
    public static Dictionary<short, DataItem> dictDataItems = new Dictionary<short, DataItem>();

    public short id;
    public short grade;
    public ItemType type;
    public string tagName;
    public string tagDesc;
    public string resImage;

    public void Register()
    {
        dictDataItems.Add(id, this);
    }

    public static DataItem Get(short id)
    {
        dictDataItems.TryGetValue(id, out var dataItem);
        return dataItem;
    }
}
