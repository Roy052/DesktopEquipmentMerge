using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton
{
    public Dictionary<string, Sprite> dicResourceSprites = new();
    public Dictionary<TutorialType, List<short>> dicTutorialStrs = new()
    {
        {TutorialType.Start, new List<short>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 } },
        {TutorialType.Enter, new List<short>() { 19 } },
        {TutorialType.PullEquipment, new List<short>() { 20 } },
        {TutorialType.Merge, new List<short>() { 21 } },

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
