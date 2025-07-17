using System.Collections.Generic;
using System;

public enum ExpeditionState
{
    None,
    Progress,
    Reward,
    End
}

[Serializable]
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
        set
        {
            startTimeTicks = value.Ticks;
        }
    }
}
