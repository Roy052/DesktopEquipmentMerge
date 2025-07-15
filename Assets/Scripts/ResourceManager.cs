using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton
{
    public Dictionary<string, Sprite> dicResourceSprites = new();
    public Dictionary<TutorialType, List<short>> dicTutorialStrs = new()
    {
        {TutorialType.Start, new List<short>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 } },
        {TutorialType.EnterMerge, new List<short>() { 19 } },
        {TutorialType.PullEquipment, new List<short>() { 20 } },
        {TutorialType.Merge, new List<short>() { 21 } },
        {TutorialType.BuildQuest, new List<short>() { 22, 23, 24, 25, 26 } },
        {TutorialType.EnterQuest, new List<short>() { 27 } },
        {TutorialType.ClickTrader, new List<short>() { 28 } },
        {TutorialType.ClickQuest, new List<short>() { 29 } },
        {TutorialType.ClickAccept, new List<short>() { 30 } },
        {TutorialType.ClickSubmit, new List<short>() { 31 } },
        {TutorialType.ClickReward, new List<short>() { 32, 33, 34, 35 } },
        {TutorialType.BuildOthers, new List<short>() { 36, 37, 38, 39 } },
        {TutorialType.EnterInn, new List<short>() { 40, 41, 42, 43 } },
        {TutorialType.Recruit, new List<short>() { 44, 45, 46, 47, 48, 49, 50, 51, 52, } },
        {TutorialType.EnterExpedition, new List<short>() { 53, 54, 55 } },
        {TutorialType.Expedition, new List<short>() { 56, 57, 58, 59, 60, 61, 62, 63, 64 } },
        {TutorialType.ExpeditionMember, new List<short>() { 65 } },
        {TutorialType.ClickExpedition, new List<short>() { 66 } },
        {TutorialType.ClickExpeditionReward, new List<short>() { 67, 68 } },
        {TutorialType.End, new List<short>() { 69, 70, 71, 72, 73, 74, 75 } },
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
