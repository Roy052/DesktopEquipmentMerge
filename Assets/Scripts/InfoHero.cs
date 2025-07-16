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
    public int hirePrice;
    public int price;
    public HeroState state;
    public InjuryType injury;
}
