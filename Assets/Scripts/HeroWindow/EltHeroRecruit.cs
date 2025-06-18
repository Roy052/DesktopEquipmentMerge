using UnityEngine;

public class EltHeroRecruit : EltHero
{
    
    public void OnClickRecruit()
    {
        Singleton.gm.gameData.AddInfoUnit(infoUnit);
    }
}
