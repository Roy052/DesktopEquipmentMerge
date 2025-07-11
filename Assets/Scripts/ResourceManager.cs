using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton
{
    public Dictionary<string, Sprite> dicResourceSprites = new();
    public Dictionary<TutorialType, List<short>> dicTutorialStrs = new()
    {
        {TutorialType.Start, new List<short>() {0, } }
    };
    public Material mat_GrayScale;

    public void Awake()
    {
        resourceManager = this;
        Resources.LoadAll("Arts");
        var sprites = Resources.FindObjectsOfTypeAll<Sprite>();
        foreach(var sprite in sprites)
        {
            dicResourceSprites.Add(sprite.name, sprite);
        }
    }
}
