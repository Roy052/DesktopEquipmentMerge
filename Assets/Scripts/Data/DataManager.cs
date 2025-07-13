using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using System;

public class DataManager : Singleton
{
    enum SheetName
    {
        Item,
        MergeItem,
        Building,
        Trader,
        TextTag,
        Quest,
        Expedition,
        RewardProb,
        Hero,
        Lv,
        Dialog,
    }

    const string pathHead = "./Assets/Data/";
    const string pathTail = ".dem";

    [System.Serializable]
    public class DataList<T>
    {
        public List<T> data;
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (dataManager == null)
            dataManager = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SetDatas();
    }

    public void SetDatas()
    {
        LoadFromJson<DataItem>(SheetName.Item);
        LoadFromJson<DataMergeItem>(SheetName.MergeItem);
        LoadFromJson<DataBuilding, TempDataBuilding>(SheetName.Building);
        LoadFromJson<DataTrader, TempDataTrader>(SheetName.Trader);
        LoadFromJson<DataTextTag>(SheetName.TextTag);
        LoadFromJson<DataQuest, TempDataQuest>(SheetName.Quest);
        LoadFromJson<DataExpedition, TempDataExpedition>(SheetName.Expedition);
        LoadFromJson<DataRewardProb, TempDataRewardProb>(SheetName.RewardProb);
        LoadFromJson<DataHero>(SheetName.Hero);
        LoadFromJson<DataLv>(SheetName.Lv);
        LoadFromJson<DataDialog>(SheetName.Dialog);
    }

    void LoadFromJson<T>(SheetName sheet) where T : IRegistrable
    {
        string filePath = $"{pathHead}{sheet}{pathTail}";

        try
        {
            if (!File.Exists(filePath)) return;

            string jsonText = File.ReadAllText(filePath);
            var dataList = JsonUtility.FromJson<DataList<T>>($"{{\"data\":{jsonText}}}");

            foreach (var item in dataList.data)
                item.Register();
        }
        catch (Exception e) when (
            e is FileNotFoundException ||
            e is DirectoryNotFoundException ||
            e is IOException)
        {
            Debug.Log($"[JSON Load Error] {e.Message}");
        }
    }

    void LoadFromJson<T, U>(SheetName sheet) where T : IRegistrable where U : IConvertable<T>
    {
        string filePath = $"{pathHead}{sheet}{pathTail}";

        try
        {
            if (!File.Exists(filePath)) return;

            string jsonText = File.ReadAllText(filePath);
            var dataList = JsonUtility.FromJson<DataList<U>>($"{{\"data\":{jsonText}}}");

            foreach (var item in dataList.data)
            {
                T convertedItem = item.ConvertTo();
                convertedItem.Register();
            }
        }
        catch (Exception e) when (
            e is FileNotFoundException ||
            e is DirectoryNotFoundException ||
            e is IOException)
        {
            Debug.Log($"[JSON Load Error] {e.Message}");
        }
    }
}
