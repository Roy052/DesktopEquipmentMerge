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
    public long startTimeTicks;
    public List<int> heroIdxes;

    public DateTime startTime
    {
        get
        {
            return new DateTime(startTimeTicks);
        }
    }
}
