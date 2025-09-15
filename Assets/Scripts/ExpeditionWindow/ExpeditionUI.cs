using System.Collections.Generic;
using UnityEngine;

public class ExpeditionUI : WindowUI
{
    public EltExpedition eltExpedition;
    List<EltExpedition> eltExpeditionList = new List<EltExpedition>();

    public void Start()
    {
        Show();
        Observer.onRefreshExpedition += RefreshExpedition;
    }

    public void OnDestroy()
    {
        Observer.onRefreshExpedition -= RefreshExpedition;
    }

    public override void Show()
    {
        base.Show();
        RefreshExpedition();
    }

    void RefreshExpedition()
    {
        var dataList = DataExpedition.GetAll();
        short lv = Singleton.gm.gameData.buildingLvs[(int)BuildingType.ExpeditionCamp];
        for (int i = 0; i < dataList.Count; i++)
        {
            if (lv < dataList[i].buildingLv)
                break;

            var elt = Utilities.GetOrCreate(eltExpeditionList, i, eltExpedition.gameObject);
            elt.Set(dataList[i]);
            elt.SetActive(true);
        }
        Utilities.DeactivateSurplus(eltExpeditionList, dataList.Count);
    }
}
