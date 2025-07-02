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

public class InfoQuest : MonoBehaviour
{
    public QuestState state;
    public short questId;
    public List<int> questProgress;
}
