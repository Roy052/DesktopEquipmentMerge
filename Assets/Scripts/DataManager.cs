using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class DataManager : Singleton
{
    enum SheetName
    {
        Unit,
        UnitGame,
        Label,
    }

    const string pathHead = "./Assets/Data/";
    const string pathTail = ".hpm";

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
        var unitDatas = LoadFromJson_Unit();
        foreach (var unitData in unitDatas)
            CollectableData.collectableDatas.Add(unitData.id, unitData);

    }

    List<CollectableData> LoadFromJson_Unit()
    {
        try
        {
            //Debug.Log(path);
            if (File.Exists(pathHead + SheetName.Unit + pathTail))
            {
                string jsonText = File.ReadAllText(pathHead + SheetName.Unit + pathTail);
                DataList<TempCollectableData> dataList = JsonUtility.FromJson<DataList<TempCollectableData>>("{\"data\":" + jsonText + "}");
                List<CollectableData> unitDatas = new List<CollectableData>();
                foreach (var tempUnitData in dataList.data)
                    unitDatas.Add(tempUnitData.Change());
                return unitDatas;
            }
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("The file was not found:" + e.Message);
            return default;
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("The directory was not found: " + e.Message);
            return default;
        }
        catch (IOException e)
        {
            Debug.Log("The file could not be opened:" + e.Message);
            return default;
        }
        return default;
    }
}
