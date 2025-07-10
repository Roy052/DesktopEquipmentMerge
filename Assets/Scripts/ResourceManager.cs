using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EquipmentIdSprite
{
    public int id;
    public Sprite sprite;
}

public class ResourceManager : Singleton
{
    public EquipmentIdSprite[] equipmentSprites;
    public Dictionary<int, Sprite> dicEquipmentSprites = new Dictionary<int, Sprite>();
    public Dictionary<string, Sprite> dicResourceSprites = new();
    public Dictionary<TutorialType, List<short>> dicTutorialStrs = new()
    {
        {TutorialType.Start, new List<short>() {0, } }
    };
    public Material mat_GrayScale;

    public void Awake()
    {
        resourceManager = this;
        foreach (var equipmentSprite in equipmentSprites)
            dicEquipmentSprites[equipmentSprite.id] = equipmentSprite.sprite;

        var sprites = Resources.FindObjectsOfTypeAll<Sprite>();
        foreach(var sprite in sprites)
        {
            dicResourceSprites.Add(sprite.name, sprite);
        }
    }


}
