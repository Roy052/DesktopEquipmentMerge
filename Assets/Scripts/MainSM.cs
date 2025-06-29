using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainSM : Singleton
{
    [Header("Buildings")]
    public List<EltBuilding> buildings;

    private void Awake()
    {
        mainSM = this;
        Observer.onRefreshBuilding += RefreshBuilding;
    }

    private void OnDestroy()
    {
        mainSM = null;
        Observer.onRefreshBuilding -= RefreshBuilding;
    }

    public void Set()
    {
        RefreshBuilding();
    }

    public void OnClickBuild(BuildingType type)
    {
        gm.gameData.BuildBuilding(type);
    }

    void RefreshBuilding()
    {
        var buildingLvs = gm.gameData.buildingLvs;
        for (int i = 0; i < buildingLvs.Length; i++)
        {
            buildings[i].Set((BuildingType)i, buildingLvs[i] == 0);
            buildings[i].funcBuild = OnClickBuild;
        }
    }
}
