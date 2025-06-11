using System;
using System.Collections.Generic;

[Serializable]
public class DataEquipment : IRegistrable
{
    public static Dictionary<short, DataEquipment> dictDataEquipments;

    public short id;
    public string tagName;
    public short grade;
    public string resImage;
    public long price;

    public void Register()
    {
        dictDataEquipments.Add(id, this);
    }
}
