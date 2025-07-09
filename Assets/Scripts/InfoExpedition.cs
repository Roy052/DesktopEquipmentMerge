using System.Collections.Generic;
using System;
using UnityEngine;

public enum ExpeditionState
{
    None,
    Progress,
    Reward,
    End
}

[System.Serializable]
public class InfoExpedition
{
    public int expeditionUid;
    public short expeditionId;
    public ExpeditionState state;
    public DateTime startTime;
    public List<int> heroIdxes;
}
