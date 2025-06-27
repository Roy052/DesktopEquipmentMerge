using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltChest : MonoBehaviour
{
    public Text textCount;
    [NonSerialized]public UnityAction<MergeItemCategory> funcClick;

    MergeItemCategory category;

    public void Set(MergeItemCategory category)
    {
        this.category = category;
        Refresh();
    }

    public void OnClickChest()
    {
        funcClick?.Invoke(category);
    }

    public void Refresh()
    {
        Singleton.gm.gameData.storedEquipmentList.TryGetValue(category, out var list);
        textCount.text = (list?.Count ?? 0).ToString();
    }
}
