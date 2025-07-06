using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltQuest : MonoBehaviour
{
    public Text textName;
    public GameObject objClear;
    public GameObject objProgress;
    public GameObject objSelected;

    public UnityAction<InfoQuest> funcClick;

    InfoQuest info;

    public void Set(InfoQuest info)
    {
        if (info.state == QuestState.NotOpened)
            return;

        this.info = info;
        DataQuest data = DataQuest.Get(info.questId);
        textName.text = DataTextTag.FindText(data.tagName);

        objClear.SetActive(info.state == QuestState.Clear);
        objProgress.SetActive(info.state == QuestState.Progress);
    }

    public void OnClickQuest()
    {
        funcClick?.Invoke(info);
    }

    public int QuestId => info.questId;
}
