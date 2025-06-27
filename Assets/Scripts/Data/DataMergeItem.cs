using System;
using System.Collections.Generic;

public enum MergeItemCategory
{
    All = -1,
    WeaponWarrior,
    WeaponArcher,
    WeaponRogue,
    WeaponWizard,
    Armor,
    Cloth,
    Max,
}

public enum MergeItemType
{
    Sword,
    Axe,
    Bow,
    Crossbow,
    Dagger,
    Rapier,
    Stick,
    Relic,
    ExpBook,
    MoneyBag,
}

[Serializable]
public class DataMergeItem : IRegistrable
{
    public static Dictionary<short, DataMergeItem> dictDataMergeItems = new Dictionary<short, DataMergeItem>();

    public short id;
    public MergeItemType type;
    public MergeItemCategory category;
    public int price;

    public void Register()
    {
        dictDataMergeItems.Add(id, this);
    }

    public static DataMergeItem Get(short id)
    {
        dictDataMergeItems.TryGetValue(id, out DataMergeItem item);
        return item;
    }
}
