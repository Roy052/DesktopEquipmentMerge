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

    public void Awake()
    {
        resourceManager = this;
        foreach (var equipmentSprite in equipmentSprites)
            dicEquipmentSprites[equipmentSprite.id] = equipmentSprite.sprite;
    }


}
