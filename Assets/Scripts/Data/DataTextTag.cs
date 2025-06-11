using System;
using System.Collections.Generic;

[Serializable]
public class DataTextTag : IRegistrable
{
    public static Dictionary<string, DataTextTag> dictDataTexts;

    public string tag;
    public string tagKor;
    public string tagEng;

    public void Register()
    {
        dictDataTexts.Add(tag, this);
    }
}
