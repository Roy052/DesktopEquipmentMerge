using System.Collections.Generic;
using UnityEngine;

public class ExpeditionUI : WindowUI
{
    public EltExpedition eltExpedition;
    List<EltExpedition> eltExpeditionList = new List<EltExpedition>();

    public void Start()
    {
        Show();
    }

    public override void Show()
    {
        base.Show();
        var dataList = DataExpedition.GetAll();
        for(int i = 0; i < dataList.Count; i++)
        {
            var elt = Utilities.GetOrCreate(eltExpeditionList, i, eltExpedition.gameObject);
            elt.Set(dataList[i]);
            elt.SetActive(true);
        }
        Utilities.DeactivateSurplus(eltExpeditionList, dataList.Count);
    }
}
