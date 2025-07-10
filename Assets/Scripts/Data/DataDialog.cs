using System;
using System.Collections.Generic;

[Serializable]
public class DataDialog : IRegistrable
{
    public static Dictionary<short, DataDialog> dictDataDialogs = new Dictionary<short, DataDialog>();

    public short id;
    public string tagName;
    public string tagText;

    public void Register()
    {
        dictDataDialogs.Add(id, this);
    }

    public static DataDialog Get(short id)
    {
        dictDataDialogs.TryGetValue(id, out var value);
        return value;
    }
}


