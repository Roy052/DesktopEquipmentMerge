using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestWindow : GameWindow
{
    public EltTrader eltTrader;
    public EltQuest eltQuest;

    [Header("QuestDesc")]
    public GameObject objQuestDescContents;
    public Text textQuestName;
    public Text textQuestDesc;
    public LayoutElement layoutProgress;
    public GameObject objBtnAccept;
    public GameObject objBtnReward;
    public EltQuestProgress eltQuestProgress;
    List<EltQuestProgress> questProgresses = new List<EltQuestProgress>();

    List<EltTrader> eltTraders = new List<EltTrader>();
    List<EltQuest> eltQuests = new List<EltQuest>();

    TraderType currentTraderType = TraderType.None;
    InfoQuest currentInfoQuest = null;
    DataQuest currentDataQuest = null;

    public TraderType GetTraderType() => currentTraderType;

    public DataQuest GetDataQuest() => currentDataQuest;

    public InfoQuest GetInfoQuest() => currentInfoQuest;

    protected override void Awake()
    {
        base.Awake();
        questWindow = this;
        Observer.onRefreshQuests += Set;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        questWindow = null;
        Observer.onRefreshQuests -= Set;
    }

    public override void Show()
    {
        base.Show();
        Set();
    }

    public void Set()
    {
        eltTrader.SetActive(false);
        eltQuest.SetActive(false);
        eltQuestProgress.SetActive(false);
        objQuestDescContents.SetActive(false);

        int tIdx = 0;
        foreach (var (type, exp) in Singleton.gm.gameData.traderExps)
        {
            if (exp == 0)
                continue;
            short lv = DataLv.GetTraderLv(exp);
            var dataQuestByTrader = DataQuest.GetAllByTrader(type);
            if (dataQuestByTrader == null || dataQuestByTrader.Count == 0)
                continue;

            var elt = Utilities.GetOrCreate(eltTraders, tIdx, eltTrader.gameObject);
            elt.Set(DataTrader.Get(type, lv), lv);
            elt.funcClick = OnClickTrader;
            elt.SetActive(true);
            tIdx++;
        }
        Utilities.DeactivateSurplus(eltTraders, tIdx);

        if (currentTraderType != TraderType.None)
            OnClickTrader(currentTraderType);
    }

    public void OnClickTrader(TraderType traderType)
    {
        currentTraderType = traderType;

        var infoQuests = gm.gameData.traderQuests[traderType];
        var dataQuestByTrader = DataQuest.GetAllByTrader(traderType);
        HashSet<int> progressQuest = new HashSet<int>();

        int count = 0;
        for (int i = 0; i < dataQuestByTrader.Count; i++)
        {
            if (infoQuests[i].state == QuestState.NotOpened)
                continue;

            var elt = Utilities.GetOrCreate(eltQuests, count, eltQuest.gameObject);
            elt.Set(infoQuests[i]);
            elt.funcClick = OnClickQuest;
            elt.SetActive(true);
            count++;
        }
        Utilities.DeactivateSurplus(eltQuests, count);

        if (currentInfoQuest != null)
            OnClickQuest(currentInfoQuest);

        foreach(var elt in eltTraders)
            elt.objSelected.SetActive(elt.TraderType == traderType);
    }

    public void OnClickQuest(InfoQuest info)
    {
        if (info.state == QuestState.NotOpened)
            return;

        currentInfoQuest = info;
        objQuestDescContents.SetActive(true);

        DataQuest data = DataQuest.Get(info.questId);
        currentDataQuest = data;
        textQuestName.text = DataTextTag.FindText(data.tagName);
        textQuestDesc.text = DataTextTag.FindText(data.tagDesc);

        if(info.state != QuestState.NotAccept)
        {
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
            layoutProgress.preferredHeight = count * 50f;
        }
        
        objBtnAccept.SetActive(info.state == QuestState.NotAccept);
        objBtnReward.SetActive(info.state == QuestState.Reward);

        foreach(var elt in eltQuests)
            elt.objSelected.SetActive(elt.QuestId == currentInfoQuest.questId);
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
        List<ItemIdCount> items = new()
        {
            new ItemIdCount() { itemId = itemId, itemCount = submitCount }
        };
        gm.gameData.PayCost(items);

        bool isComplete = true;
        for (int i = 0; i < currentInfoQuest.questProgress.Count; i++)
        {
            if (currentInfoQuest.questProgress[i] < currentDataQuest.requireItems[i].itemCount)
            {
                isComplete = false;
                break;
            }
        }

        if (isComplete)
            currentInfoQuest.state = QuestState.Reward;

        Set();
    }

    public void OnClickAccept()
    {
        currentInfoQuest.state = QuestState.Progress;
        Set();
    }

    public void OnClickReward()
    {
        gm.gameData.OnRewardQuest(currentInfoQuest);
        Set();
    }
}
