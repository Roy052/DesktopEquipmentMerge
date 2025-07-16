using System;
using System.Collections.Generic;

[Serializable]
public class DataDialog : IRegistrable
{
    public static Dictionary<short, DataDialog> dictDataDialogs = new();
    public static Dictionary<TutorialType, List<DataDialog>> dictDataDialogsByType = new();

    public short id;
    public TutorialType tutorialType;
    public string tagName;
    public string tagText;

    public void Register()
    {
        dictDataDialogs.Add(id, this);

        if (dictDataDialogsByType.TryGetValue(tutorialType, out var list))
            list.Add(this);
        else
            dictDataDialogsByType.Add(tutorialType, new List<DataDialog>() { this });
    }

    public static DataDialog Get(short id)
    {
        dictDataDialogs.TryGetValue(id, out var value);
        return value;
    }

    public static List<DataDialog> GetListByType(TutorialType type)
    {
        dictDataDialogsByType.TryGetValue(type, out var list);
        return list;
    }
}


