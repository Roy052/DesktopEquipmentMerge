using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    NotOpened,
    NotAccept,
    Progress,
    Reward,
    Clear
}

public class InfoQuest
{
    public QuestState state;
    public int questId;
    public List<int> questProgress;
}
