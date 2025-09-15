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
        mainCanvas = GetComponent<Canvas>();
        Observer.onRefreshBuilding += RefreshBuilding;
        Observer.onRefreshItems += RefreshBuilding;
    }

    private void OnDestroy()
    {
        mainSM = null;
        mainCanvas = null;
        Observer.onRefreshBuilding -= RefreshBuilding;
        Observer.onRefreshItems -= RefreshBuilding;
    }

    public void Set()
    {
        RefreshBuilding();
    }

    public void OnClickBuild(BuildingType type)
    {
        gm.gameData.BuildBuilding(type);
    }

    public void OnClickLevelup(BuildingType type)
    {
        gm.gameData.LevelupBuilding(type);
    }

    void RefreshBuilding()
    {
        var buildingLvs = gm.gameData.buildingLvs;
        for (int i = 0; i < buildingLvs.Length; i++)
        {
            buildings[i].Set((BuildingType)i, buildingLvs[i]);
            buildings[i].funcBuild = OnClickBuild;
        }
    }
}
