using System;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject objLock;

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
            textExpeditionRemainTime.text = $"{(int)(info.startTime.AddMinutes(data.expeditionTime) - DateTime.Now).TotalSeconds} √ ";
        }

        bool isLock = true;
        if (dataExpedition.missionConditionId == -1 || Singleton.gm.gameData.IsMissionClear(dataExpedition.missionConditionId))
            isLock = false;
        objLock.SetActive(isLock);
    }

    public void OnClickSelectExpeditionMember()
    {
        Singleton.expeditionWindow.ShowExpeditionMember(dataExpedition);
    }

    public void OnclickGetRewardExpedition()
    {
        Singleton.gm.gameData.GetRewardExpedition(dataExpedition.id);
    }
}
