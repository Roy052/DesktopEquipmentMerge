using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeroMemberEquipmentUI : WindowUI
{
    public UnityAction<int> funcClick;
    public EltItem eltItem;

    List<EltItem> eltItemList = new();
    List<int> itemIdList;

    public void Awake()
    {
        Observer.onRefreshMergeWindow += Hide;
    }

    public void OnDestroy()
    {
        Observer.onRefreshMergeWindow -= Hide;
    }

    public void Set(MergeItemType type)
    {
        itemIdList = Singleton.gm.gameData.GetMergeItemListByType(type);
        for (int i = 0; i < itemIdList.Count; i++)
        {
            var elt = Utilities.GetOrCreate(eltItemList, i, eltItem.gameObject);
            elt.Set(itemIdList[i], i);
            elt.funcClick = OnClickItem;
            elt.SetActive(true);
        }

        Utilities.DeactivateSurplus(eltItemList, itemIdList.Count);
    }

    public void OnClickItem(int idx)
    {
        funcClick?.Invoke(itemIdList[idx]);
        Hide();
    }
}
