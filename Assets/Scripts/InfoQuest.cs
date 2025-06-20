using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    NotOpened,
    Progress,
    Reward,
    End
}

public class InfoQuest : MonoBehaviour
{
    public QuestState state;
    public short questId;
    public List<int> questProgress;
}
