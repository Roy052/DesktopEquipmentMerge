using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EltExpedition : MonoBehaviour
{
    public Text textName;
    public Image imgRes;
    public Text textEquipmentCount;
    public Text textRewardProb;

    DataExpedition dataExpedition;

    public void Set(DataExpedition data)
    {
        dataExpedition = data;
        if (textName)
            textName.text = DataTextTag.FindText(data.tagName);
    }

    public void OnClickGoExpedition()
    {
        List<int> temp = new List<int>();
        for (int i = 0; i < dataExpedition.equipmentCount; i++)
            temp.Add(1);
        
        Singleton.gm.gameData.AddItems(temp);
    }
}
