using UnityEngine;
using System.Collections.Generic;

public class TitleUI : MonoBehaviour
{
    [Header("Button")]
    public ButtonAddOn btnLoad;

    [Header("Option")]
    public OptionUI optionUI;

    //[Header("Save List")]
    //public SaveElt saveElt;
    //List<SaveElt> saveEltList = new();

    GameData save;

    public void Open()
    {
        save = GameData.Load();
        if (save == null)
        {
            btnLoad.ChangeGrayScale(true);
            btnLoad.btn.enabled = false;
        }
    }

    public void OnClickPlay()
    {
        if (save != null)
        {
            Singleton.msgBox.SetActive(true);
            Singleton.msgBox.SetYesNo("Notice", "Your current progress will be reset. Do you want to continue?", () => { Singleton.gm.OnClickPlay(); });
        }
        else
        {
            Singleton.gm.OnClickPlay();
        }
    }

    public void OnClickLoad()
    {
        Singleton.gm.gameData = save;
        Observer.RefreshAll();
    }

    public void OnClickOption()
    {
        optionUI.SetActive(true);
        optionUI.Set();
    }

    public void OnClickQuit()
    {
        GameData.Save(Singleton.gm.gameData);
        Application.Quit();
    }

    //void LoadList()
    //{
    //    var gameDataAuto = GameData.Load(true, 0);
    //    if(gameDataAuto == null)
    //    {
    //        btnLoad.ChangeGrayScale(true);
    //        btnLoad.btn.enabled = false;
    //    }

    //    if(saveEltList.Count == 0)
    //    {
    //        SaveElt tempElt = Instantiate(saveElt.gameObject, saveElt.transform.parent).GetComponent<SaveElt>();
    //        saveEltList.Add(tempElt);
    //    }
    //    saveEltList[0].Set($"AutoSave {gameDataAuto.saveTime}", 0);

    //    for(int i = 0; i < 3; i++)
    //    {

    //    }
    //}
}
