using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    Dictionary<int, long> dicItems = new Dictionary<int, long>();
    public List<int> storedEquipmentList = new List<int>();

    public List<InfoUnit> infoUnits = new List<InfoUnit>();

    public void AddItems(List<int> items)
    {
        foreach(var itemId in items)
        {
            if (itemId == 1)
                storedEquipmentList.Add(itemId);

            if (dicItems.ContainsKey(itemId))
                dicItems[itemId] += 1;
            else
                dicItems[itemId] = 1;
        }
    }

    public void AddInfoUnit(InfoUnit infoUnit)
    {
        infoUnits.Add(infoUnit);
    }
}
