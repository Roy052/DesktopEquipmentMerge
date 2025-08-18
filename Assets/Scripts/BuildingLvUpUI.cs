using System.Collections.Generic;
using UnityEngine;

public class BuildingLvUpUI : MonoBehaviour
{
    public ButtonAddOn btnLvUp;
    public SlotItem slotRequireItem;
    List<SlotItem> slotRequireItems = new();

    BuildingType type;

    public void Set(BuildingType type, short lv)
    {
        this.type = type;

        DataBuilding dataBuilding = DataBuilding.Get(type, lv);
        if (dataBuilding == null)
        {
            Debug.LogError($"Data Building Not Exist {type} {lv}");
            return;
        }

        bool isConditionClear = Singleton.gm.gameData.IsConditionClear(dataBuilding.conditions);
        btnLvUp.ChangeGrayScale(isConditionClear == false);
        btnLvUp.enabled = isConditionClear;

        slotRequireItem.SetActive(false);
        for (int i = 0; i < dataBuilding.requireItems.Count; i++)
        {
            var requireItem = dataBuilding.requireItems[i];
            if (requireItem.itemId == -1)
                continue;

            var elt = Utilities.GetOrCreate(slotRequireItems, i, slotRequireItem.gameObject);
            elt.Set(requireItem.itemId, requireItem.itemCount);
            elt.SetActive(true);
        }

        Utilities.DeactivateSurplus(slotRequireItems, dataBuilding.requireItems.Count);
    }

    public void OnClickLvUp()
    {
        Singleton.gm.gameData.LevelupBuilding(type);
    }
}
