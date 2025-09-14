using UnityEngine;
using System.Collections.Generic;

public class TitleUI : MonoBehaviour
{
    [Header("Button")]
    public ButtonAddOn btnLoad;

    [Header("Option")]
    public OptionUI optionUI;

    GameData save;

    public void Open()
    {
        save = GameData.Load();
        if (save == null)
        {
            btnLoad.ChangeGrayScale(true);
            btnLoad.btn.enabled = false;
        }
        optionUI.SetActive(false);
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
        Singleton.gm.OnClickLoad(save);
        Observer.RefreshAll();
    }

    public void OnClickOption()
    {
        optionUI.SetActive(!optionUI.gameObject.activeSelf);
        optionUI.Set();
    }

    public void OnClickQuit()
    {
        GameData.Save(Singleton.gm.gameData);
        Application.Quit();
    }
}
