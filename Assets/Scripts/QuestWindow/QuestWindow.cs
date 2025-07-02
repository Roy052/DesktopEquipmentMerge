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

    public override void Show()
    {
        base.Show();
        if (eltTrader == null)
        {
            Debug.Log($"EltTrader Not Exist");
            return;
        }
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
        if (eltQuest == null)
        {
            Debug.Log($"EltQuest Not Exist");
            return;
        }
        var infoQuests = gm.gameData.infoQuests.Where(x => (DataQuest.Get(x.questId)?.traderType ?? TraderType.None) == traderType).ToList();
        var dataQuestByTrader = DataQuest.GetAllByTrader(traderType);
        HashSet<int> progressQuest = new HashSet<int>();

        int count = 0;
        for (int i = 0; i < dataQuestByTrader.Count; i++)
        {
            if (gm.gameData.IsConditionClear(dataQuestByTrader[i].id) == false)
                continue;

            var elt = Utilities.GetOrCreate(quests, count, eltQuest.gameObject);
            elt.Set(infoQuests[i]);
            elt.SetActive(true);
        }
        Utilities.DeactivateSurplus(quests, count);
    }

    public void OnClickQuest(InfoQuest info)
    {
        if (info.state == QuestState.NotOpened)
            return;

        DataQuest data = DataQuest.Get(info.questId);
        textQuestName.text = DataTextTag.FindText(data.tagName);
        textQuestDesc.text = DataTextTag.FindText(data.tagDesc);

        if (eltQuestProgress == null)
        {
            Debug.LogError("EltQuestProgress Not Exist");
            return;
        }

        for (int i = 0; i < data.requireItems.Count; i++)
        {
            var elt = Utilities.GetOrCreate(questProgresses, i, eltQuestProgress.gameObject);
            elt.Set(data.requireItems[i].itemId, info.questProgress[i], data.requireItems[i].itemCount);
            elt.SetActive(true);
        }

        Utilities.DeactivateSurplus(questProgresses, data.requireItems.Count);
    }
}
