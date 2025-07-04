using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class QuestWindow : GameWindow
{
    public EltTrader eltTrader;
    public EltQuest eltQuest;

    [Header("QuestDesc")]
    public Text textQuestName;
    public Text textQuestDesc;
    public EltQuestProgress eltQuestProgress;
    List<EltQuestProgress> questProgresses = new List<EltQuestProgress>();

    List<EltTrader> traders = new List<EltTrader>();
    List<EltQuest> quests = new List<EltQuest>();

    InfoQuest currentInfoQuest;
    DataQuest currentDataQuest;

    public override void Show()
    {
        base.Show();
        eltTrader.SetActive(false);
        eltQuest.SetActive(false);
        eltQuestProgress.SetActive(false);

        int tIdx = 0;
        gm.gameData.traderLvs.Add(TraderType.DrakoKingdom, 1);
        foreach (var (type, lv) in Singleton.gm.gameData.traderLvs)
        {
            if (lv == 0)
                continue;

            var elt = Utilities.GetOrCreate(traders, tIdx, eltTrader.gameObject);
            elt.Set(DataTrader.Get(type, lv), lv);
            elt.funcClick = OnClickTrader;
            elt.SetActive(true);
            tIdx++;
        }
        Utilities.DeactivateSurplus(traders, tIdx);

    }

    public void OnClickTrader(TraderType traderType)
    {
        var infoQuests = gm.gameData.traderQuests[traderType];
        var dataQuestByTrader = DataQuest.GetAllByTrader(traderType);
        HashSet<int> progressQuest = new HashSet<int>();

        int count = 0;
        for (int i = 0; i < dataQuestByTrader.Count; i++)
        {
            if (infoQuests[i].state == QuestState.NotOpened)
                continue;

            var elt = Utilities.GetOrCreate(quests, count, eltQuest.gameObject);
            elt.Set(infoQuests[i]);
            elt.funcClick = OnClickQuest;
            elt.SetActive(true);
            count++;
        }
        Utilities.DeactivateSurplus(quests, count);
    }

    public void OnClickQuest(InfoQuest info)
    {
        if (info.state == QuestState.NotOpened)
            return;

        currentInfoQuest = info;

        DataQuest data = DataQuest.Get(info.questId);
        currentDataQuest = data;
        textQuestName.text = DataTextTag.FindText(data.tagName);
        textQuestDesc.text = DataTextTag.FindText(data.tagDesc);

        int count = 0;
        for (int i = 0; i < data.requireItems.Count; i++)
        {
            if (data.requireItems[i].itemId == -1)
                continue;

            var elt = Utilities.GetOrCreate(questProgresses, i, eltQuestProgress.gameObject);
            elt.Set(count, data.requireItems[i].itemId, info.questProgress[i], data.requireItems[i].itemCount);
            elt.funcSubmit = OnQuestSubmit;
            elt.SetActive(true);
            count++;
        }

        Utilities.DeactivateSurplus(questProgresses, count);
    }

    public void OnQuestSubmit(int idx)
    {
        if (currentInfoQuest == null)
            return;

        int itemId = currentDataQuest.requireItems[idx].itemId;
        int currentCount = gm.gameData.GetCountMergeItemId(itemId);
        if (currentCount == 0)
            return;

        int currentRequireItem = currentDataQuest.requireItems[idx].itemCount - currentInfoQuest.questProgress[idx];
        int submitCount = System.Math.Min(currentCount, currentRequireItem);

        currentInfoQuest.questProgress[idx] += submitCount;
        gm.gameData.RemoveMergeItem(itemId, submitCount);
    }
}
