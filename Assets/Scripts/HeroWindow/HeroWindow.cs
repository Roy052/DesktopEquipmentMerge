using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroWindow : GameWindow
{
    public enum HeroUI
    {
        HeroRecruit,
        HeroMember,
    }

    public Toggle[] tabs;
    public WindowUI[] uis;

    public override void Show()
    {
        base.Show();
        
        transform.SetAsLastSibling();
    }

    public void OnToggleHeroRecruit(bool isOn)
    {
        if (isOn == false)
            return;

        for (int i = 0; i < uis.Length; i++)
            uis[i].Hide();

        uis[(int)HeroUI.HeroRecruit].Show();
    }

    public void OnToggleHeroMember(bool isOn)
    {
        if (isOn == false)
            return;

        for (int i = 0; i < uis.Length; i++)
            uis[i].Hide();

        uis[(int)HeroUI.HeroMember].Show();
    }
}
