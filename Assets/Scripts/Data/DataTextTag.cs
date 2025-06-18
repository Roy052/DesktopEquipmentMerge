using System;
using System.Collections.Generic;

[Serializable]
public class DataTextTag : IRegistrable
{
    public static Dictionary<string, DataTextTag> dictDataTexts = new Dictionary<string, DataTextTag>();

    public string textTag;
    public string textKor;
    public string textEng;

    public void Register()
    {
        dictDataTexts.Add(textTag, this);
    }

    public static string FindText(string tag)
    {
        dictDataTexts.TryGetValue(tag, out var value);
        return value.textKor;
    }
}
