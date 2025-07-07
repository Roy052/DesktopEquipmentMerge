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
        textExpeditionRemainTime.SetActive(info != null);
        if (info != null)
        {
            btnGetReward.enabled = info.state == ExpeditionState.Reward;
            textExpeditionState.text = DataTextTag.FindText($"Expedition_State_{info.state}");

            if(info.state == ExpeditionState.Progress || info.state == ExpeditionState.Reward)
            {
                int remainSec = (int)(info.startTime.AddMinutes(data.expeditionTime) - DateTime.Now).TotalSeconds;
                if (remainSec <= 0)
                    remainSec = 0;
                textExpeditionRemainTime.text = $"{remainSec} ��";
            }
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

    public void OnClickGetRewardExpedition()
    {
        Singleton.gm.gameData.GetRewardExpedition(dataExpedition.id);
    }
}
