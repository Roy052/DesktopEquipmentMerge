using System;
using System.Collections.Generic;

[Serializable]
public class DataMission : IRegistrable
{
    public static Dictionary<short, DataMission> dictDataMissions;

    public short id;
    public short beforeMissionId;
    public string tagName;
    public string tagDesc;
    public int requireItemId1;
    public int requireItemCount1;
    public int requireItemId2;
    public int requireItemCount2;
    public int requireItemId3;
    public int requireItemCount3;
    public int rewardItemId1;
    public int rewardItemCount1;
    public int rewardItemId2;
    public int rewardItemCount2;
    public int rewardItemId3;
    public int rewardItemCount3;

    public void Register()
    {
        dictDataMissions.Add(id, this);
    }
}