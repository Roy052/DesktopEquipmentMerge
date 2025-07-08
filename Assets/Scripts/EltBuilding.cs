using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltBuilding : MonoBehaviour
{
    public Image imgBuilding;
    public Button btnBuild;
    public Button btnLvUp;
    public GameWindow gameWindow;
    public SlotItem slotRequireItem;
    List<SlotItem> slotRequireItems = new();

    public UnityAction<BuildingType> funcBuild;
    public UnityAction<BuildingType> funcLvUp;

    BuildingType type;
    int lv = 0;

    public void Set(BuildingType type, int lv)
    {
        this.type = type;
        this.lv = lv;

        imgBuilding.material = lv == 0 ? Singleton.resourceManager.mat_GrayScale : null;
        btnBuild.SetActive(lv == 0);
        btnLvUp.SetActive(lv != 0);

        slotRequireItem.SetActive(false);
        DataBuilding dataBuilding = DataBuilding.Get(type, lv);
        if(dataBuilding == null)
        {
            Debug.LogError($"Data Building Not Exist {type} {lv}");
            return;
        }

        for(int i = 0; i < dataBuilding.requireItems.Count; i++)
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

    public void OnClickLvUp()
    {
        funcLvUp?.Invoke(type);
    }
}
