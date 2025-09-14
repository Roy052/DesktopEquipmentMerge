using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HeroUI
{
    HeroRecruit,
    HeroMember,
}

public class HeroWindow : GameWindow
{
    public Toggle[] tabs;
    public WindowUI[] uis;

    protected override void Awake()
    {
        base.Awake();
        heroWindow = this;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        heroWindow = null;
    }

    public override void Show()
    {
        base.Show();
        
        transform.SetAsLastSibling();
        if (tabs[0].isOn)
            OnToggleHeroRecruit(true);
        else
            tabs[0].isOn = true;
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
