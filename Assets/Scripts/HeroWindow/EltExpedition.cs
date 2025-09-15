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
                int remainSec = (int)(info.startTime.AddSeconds(data.expeditionTime) - DateTime.Now).TotalSeconds;
                if (remainSec > 0)
                    textExpeditionRemainTime.text = $"{Utilities.GetStringRemainSec(remainSec)}";
                else
                    textExpeditionRemainTime.text = "";
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
        //Check Full
        DataRewardProb dataProb = DataRewardProb.Get(dataExpedition.rewardProbId);
        if (dataProb == null)
        {
            Debug.LogError($"DataRewardProb {dataExpedition.rewardProbId} Not Exist");
            return;
        }

        bool canFullStorage = false;
        for (int i = 0; i < dataProb.types.Count; i++)
        {
            if (dataProb.types[i] == MergeItemType.None)
                break;

            int currentCount = 0;
            MergeItemCategory category = DataMergeItem.GetCategory(dataProb.types[i]);
            if (Singleton.gm.gameData.storedEquipmentList.TryGetValue(category, out var list))
                currentCount = list.Count;
            else
                currentCount = 0;

            var dataBuilding = DataBuilding.Get(BuildingType.Storage, Singleton.gm.gameData.buildingLvs[(int)BuildingType.Storage]);
            if (dataBuilding == null)
                break;

            int maxCount = (int)dataBuilding.buildingValues[0];
            if (currentCount + dataExpedition.equipmentCount > maxCount)
            {
                canFullStorage = true;
                break;
            }
        }

        if (canFullStorage)
        {
            Singleton.msgBox.SetActive(true);
            Singleton.msgBox.SetYesNo("Notice", "Left Change By Money", () => { Singleton.gm.gameData.GetRewardExpedition(dataExpedition.id); });
        }
        else
            Singleton.gm.gameData.GetRewardExpedition(dataExpedition.id);
    }
}
