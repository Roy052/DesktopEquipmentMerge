using System;

public enum HeroState
{
    None = 0,
    InExpedition = 1,
    InInjury = 2,
}

public enum InjuryType
{
    None,
    MinorInjury,
    Injury,
    SeriousInjury
}

[System.Serializable]
public class InfoHero
{
    public short heroId;
    public string strName;
    public int exp;
    public int weaponId;
    public int armorId;
    public int hireCost;
    public HeroState state;
    public InjuryType injury;
    public long injuredTimeTicks;

    public DateTime injuredTime
    {
        get
        {
            return new DateTime(injuredTimeTicks);
        }
        set
        {
            injuredTimeTicks = value.Ticks;
        }
    }

    public int expeditionCost
    {
        get 
        {
            int currentLv = DataLv.GetLv(exp);
            double multiValue = 0.1 + Math.Pow(1.015, currentLv - 1) - 1;
            return (int)(hireCost * multiValue);
        }
    }
}
