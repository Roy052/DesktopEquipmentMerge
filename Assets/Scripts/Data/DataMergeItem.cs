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
    None = -1,
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
    public static Dictionary<int, DataMergeItem> dictDataMergeItems = new Dictionary<int, DataMergeItem>();
    public static Dictionary<MergeItemType, short> dictMaxGrades = new Dictionary<MergeItemType, short>();

    public int id;
    public MergeItemType type;
    public MergeItemCategory category;
    public short grade;
    public int price;

    public void Register()
    {
        dictDataMergeItems.Add(id, this);

        if (dictMaxGrades.TryGetValue(type, out short grade) == false)
            dictMaxGrades[type] = this.grade;
        else
            dictMaxGrades[type] = Math.Max(grade, this.grade);
    }

    public static DataMergeItem Get(int id)
    {
        dictDataMergeItems.TryGetValue(id, out DataMergeItem item);
        return item;
    }

    public static DataMergeItem GetByTypeGrade(MergeItemType type, short grade)
    {
        return Get(GetMergeItemId(type, grade));
    }

    public static bool IsMaxGrade(int id)
    {
        dictDataMergeItems.TryGetValue(id, out DataMergeItem item);
        if (item == null)
            return false;

        return item.grade >= dictMaxGrades[item.type];
    }
    
    public static DataMergeItem GetNextGrade(int id)
    {
        var item = Get(id);
        if (item == null)
            return null;

        //Is Max Grade
        if (item.grade >= dictMaxGrades[item.type])
            return null;

        short nextId = GetMergeItemId(item.type, (short)(item.grade + 1));
        return Get(nextId);
    }

    public static short GetMergeItemId(MergeItemType type, short grade)
    {
        return (short)(10000 + ((int)type * 100) + grade);
    }
}
