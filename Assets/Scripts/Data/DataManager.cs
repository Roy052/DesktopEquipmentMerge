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
        Equipment,
        Building,
        Trader,
        TextTag,
        Battle,
        RewardProb,
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
        LoadFromJson<DataEquipment>(SheetName.Equipment);
        LoadFromJson<DataBuilding>(SheetName.Building);
        LoadFromJson<DataTrader>(SheetName.Trader);
        LoadFromJson<DataTextTag>(SheetName.TextTag);
        LoadFromJson<DataExpedition>(SheetName.Battle);
        LoadFromJson<DataRewardProb>(SheetName.RewardProb);
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
}
