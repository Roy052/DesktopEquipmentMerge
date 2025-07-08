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
    public const int GoldId = 0;

    public static Dictionary<int, DataItem> dictDataItems = new Dictionary<int, DataItem>();

    public int id;
    public short grade;
    public ItemType type;
    public string tagName;
    public string tagDesc;
    public string resImage;

    public void Register()
    {
        dictDataItems.Add(id, this);
    }

    public static DataItem Get(int id)
    {
        dictDataItems.TryGetValue(id, out var dataItem);
        return dataItem;
    }
}
