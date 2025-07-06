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
    List<SlotItem> slotItems;

    public UnityAction<BuildingType> funcBuild;
    public UnityAction<BuildingType> funcLvUp;

    BuildingType type;
    int lv = 0;

    public void Set(BuildingType type, int lv)
    {
        this.type = type;

        imgBuilding.material = lv == 0 ? Singleton.resourceManager.mat_GrayScale : null;
        btnBuild.SetActive(lv == 0);
        btnLvUp.SetActive(lv != 0);

        DataBuilding dataBuilding = DataBuilding.Get(type, lv);
        for(int i = 0; i < dataBuilding.requireItems.Count; i++)
        {
            var elt = Utilities.GetOrCreate(slotItems, i, slotRequireItem.gameObject);
            elt.Set(DataItem.Get(dataBuilding.requireItems[i].itemId).resImage);
            elt.SetActive(true);
        }

        Utilities.DeactivateSurplus(slotItems, dataBuilding.requireItems.Count);
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
