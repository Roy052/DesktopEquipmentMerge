using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EltExpedition : MonoBehaviour
{
    public Text textName;
    public Image imgRes;
    public Text textEquipmentCount;
    public Text textRewardProb;

    public Button btnSelectExpeditionMember;
    public Button btnGetReward;
    public Text textExpeditionState;
    public Text textExpeditionRemainTime;

    DataExpedition dataExpedition;

    public void Set(DataExpedition data)
    {
        dataExpedition = data;
        if (textName)
            textName.text = DataTextTag.FindText(data.tagName);

        var info = Singleton.gm.gameData.GetCurrentExpedition(data.id);
        btnSelectExpeditionMember.SetActive(info == null);
        btnGetReward.SetActive(info != null);
        textExpeditionState.SetActive(info != null);
        if(info != null)
        {
            btnGetReward.enabled = info.state == ExpeditionState.Reward;
            textExpeditionState.text = DataTextTag.FindText($"Expedition_State_{info.state}");
            textExpeditionRemainTime.text = $"{(info.startTime.AddMinutes(data.expeditionTime) - DateTime.Now).TotalSeconds} √ ";
        }
    }

    public void OnClickSelectExpeditionMember()
    {
        Singleton.expeditionWindow.ShowExpeditionMember(dataExpedition);
    }

    public void OnclickGetRewardExpedition()
    {
        var info = Singleton.gm.gameData.GetCurrentExpedition(dataExpedition.id);
        if (info == null || info.state != ExpeditionState.Reward)
            return;

        info.state = ExpeditionState.End;

        Dictionary<short, long> dicItems = new Dictionary<short, long>();
        for (int i = 0; i < dataExpedition.equipmentCount; i++)
        {
            short key = (short)UnityEngine.Random.Range(0, 3);
            if (dicItems.ContainsKey(key))
                dicItems[key] += 1;
            else
                dicItems.Add(key, 1);
        }

        List<(short, long)> items = new List<(short, long)>();
        foreach (var kvp in dicItems)
            items.Add((kvp.Key, kvp.Value));

        Singleton.gm.gameData.AddItems(items);
    }
}
