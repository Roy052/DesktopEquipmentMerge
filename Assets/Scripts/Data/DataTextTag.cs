using System;
using System.Collections.Generic;

[Serializable]
public class DataTextTag : IRegistrable
{
    public static Dictionary<string, DataTextTag> dictDataTexts = new Dictionary<string, DataTextTag>();

    public string textTag;
    public string tagKor;
    public string tagEng;

    public void Register()
    {
        dictDataTexts.Add(textTag, this);
    }
}
