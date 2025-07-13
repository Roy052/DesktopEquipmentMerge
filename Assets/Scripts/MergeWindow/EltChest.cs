using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltChest : MonoBehaviour
{
    public Text textCount;
    public Image imgIcon;
    [NonSerialized]public UnityAction<MergeItemCategory> funcClick;
    public Sprite[] spriteTypes;

    MergeItemCategory category;

    public void Set(MergeItemCategory category)
    {
        this.category = category;
        imgIcon.sprite = spriteTypes[(int)category];
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
