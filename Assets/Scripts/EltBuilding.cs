using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltBuilding : MonoBehaviour
{
    public Image imgBuilding;
    public Button btnBuild;
    public GameWindow gameWindow;
    public SlotItem slotRequireItem;
    List<SlotItem> slotRequireItems = new();

    public UnityAction<BuildingType> funcBuild;

    BuildingType type;
    int lv = 0;

    public void Set(BuildingType type, int lv)
    {
        this.type = type;
        this.lv = lv;

        imgBuilding.material = lv == 0 ? Singleton.resourceManager.mat_GrayScale : null;

        btnBuild.SetActive(false);
        if (lv == 0)
        {
            slotRequireItem.SetActive(false);
            DataBuilding dataBuilding = DataBuilding.Get(type, lv);
            if (dataBuilding == null)
            {
                Debug.LogError($"Data Building Not Exist {type} {lv}");
                return;
            }

            bool isConditionClear = Singleton.gm.gameData.IsConditionClear(dataBuilding.conditions);
            btnBuild.SetActive(isConditionClear && lv == 0);

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
        else
            slotRequireItem.SetActive(false);
    }

    public void OnClick()
    {
        if (lv == 0)
            return;

        gameWindow.Show();
    }

    public void OnClickBuild()
    {
        funcBuild?.Invoke(type);
    }
}
