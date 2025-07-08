using System.Collections.Generic;
using UnityEngine.UI;

public class StorageWindow : GameWindow
{
    public SlotItem slotGold;
    public Text textGoldCount;

    public SlotItem slotItem;
    List<SlotItem> slotItemList = new List<SlotItem>();

    public override void Show()
    {
        base.Show();

        int tIdx = 0;
        foreach (var (itemId, itemCount) in Singleton.gm.gameData.itemCounts)
        {
            if(itemId == DataItem.GoldId)
            {
                textGoldCount.text = itemCount.ToString("#,0");
                continue;
            }
            var elt = Utilities.GetOrCreate(slotItemList, tIdx, slotItem.gameObject);
            elt.Set(itemId, itemCount);
            elt.SetActive(true);
            tIdx++;
        }
        Utilities.DeactivateSurplus(slotItemList, tIdx);
    }
}
