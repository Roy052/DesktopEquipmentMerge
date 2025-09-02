using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton
{
    public Dictionary<string, Sprite> dicResourceSprites = new();
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

    public Sprite GetSprite(string name)
    {
        if (dicResourceSprites.TryGetValue(name, out var sprite))
            return sprite;

        return null;
    }
}
