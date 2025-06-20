using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EltQuest : MonoBehaviour
{
    public Text textName;
    public Text textDesc;


    public void Set(InfoQuest info)
    {
        if (info.state == QuestState.NotOpened)
            return;

        DataQuest data = DataQuest.Get(info.questId);
        textName.text = DataTextTag.FindText(data.tagName);
        textDesc.text = DataTextTag.FindText(data.tagDesc);

    }
}
