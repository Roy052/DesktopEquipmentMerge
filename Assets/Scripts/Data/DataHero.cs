using System;
using System.Collections.Generic;

public enum RoleType
{
    Shield,
}

[Serializable]
public class DataHero : IRegistrable
{
    public static Dictionary<short, DataHero> dictDataHeroes = new Dictionary<short, DataHero>();

    public short id;
    public RoleType role;
    public ItemType weapon;
    public ItemType armor;

    public void Register()
    {
        dictDataHeroes.Add(id, this);
    }

    public static DataHero Get(short id)
    {
        dictDataHeroes.TryGetValue(id, out var value);
        return value;
    }
}

