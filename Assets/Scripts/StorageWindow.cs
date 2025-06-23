using System.Collections.Generic;
using UnityEngine;

public class StorageWindow : WindowUI
{
    public EltItem eltItem;

    List<EltItem> eltItemList = new List<EltItem>();

    public override void Show()
    {
        base.Show();

        int tIdx = 0;
        foreach (var kvp in Singleton.gm.gameData.itemCounts)
        {
            var elt = Utilities.GetOrCreate(eltItemList, tIdx, eltItem.gameObject);
            elt.Set(kvp.Key, (int)kvp.Value);
            elt.SetActive(true);
            tIdx++;
        }
        Utilities.DeactivateSurplus(eltItemList, tIdx);
    }
}
