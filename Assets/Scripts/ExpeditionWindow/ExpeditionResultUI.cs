using System.Collections.Generic;
using UnityEngine;

public class ExpeditionResult
{
    public List<(int, long)> itemLists;
    public List<int> heroIds;
    public List<int> heroBeforeExps;
    public int exp;
}

public class ExpeditionResultUI : WindowUI
{
    public SlotItem slotItem;
    List<SlotItem> slotItemList = new();

    public void Set(ExpeditionResult result)
    {
        slotItem.SetActive(false);

        for (int i = 0; i < result.itemLists.Count; i++)
        {
            var elt = Utilities.GetOrCreate(slotItemList, i, slotItem.gameObject);
            elt.Set(result.itemLists[i].Item1, result.itemLists[i].Item2);
            elt.SetActive(true);
        }
    }
}
