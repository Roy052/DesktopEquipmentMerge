using System.Collections.Generic;
using UnityEngine;

public class StorageWindow : WindowUI
{
    public EltItem eltItem;

    List<EltItem> eltItems = new List<EltItem>();

    public override void Show()
    {
        base.Show();

        int tIdx = 0;
        foreach (var kvp in Singleton.gm.gameData.itemCounts)
        {
            var elt = Utilities.GetOrCreate(eltItems, tIdx, eltItem.gameObject);
            elt.Set(kvp.Key, (int)kvp.Value);
            elt.SetActive(true);
            tIdx++;
        }
        Utilities.DeactivateSurplus(eltItems, tIdx);
    }
}
