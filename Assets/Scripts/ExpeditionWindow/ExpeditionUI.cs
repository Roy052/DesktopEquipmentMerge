using System.Collections.Generic;
using UnityEngine;

public class ExpeditionUI : WindowUI
{
    public GameObject objEltExpedition;
    List<EltExpedition> eltExpeditions = new List<EltExpedition>();

    public override void Show()
    {
        base.Show();
        var dataList = DataExpedition.GetAll();
        foreach (var data in dataList)
        {
            GameObject temp = Instantiate(objEltExpedition, objEltExpedition.transform.parent);
            temp.SetActive(true);
            EltExpedition tempElt = temp.GetComponent<EltExpedition>();
            tempElt.Set(data);
            eltExpeditions.Add(tempElt);
        }

    }
}
