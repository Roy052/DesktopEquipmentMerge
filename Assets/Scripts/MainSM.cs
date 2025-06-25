using UnityEngine;
using UnityEngine.UI;

public class MainSM : Singleton
{
    public Image[] imgBuildings;

    public void Set()
    {
        var buildingLvs = gm.gameData.buildingLvs;
        for(int i = 0; i < buildingLvs.Count; i++)
        {
            if (buildingLvs[i] == 0)
                imgBuildings[i].color = new Color(1, 1, 1, 0.5f);
        }
    }
}
