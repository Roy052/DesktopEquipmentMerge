using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroRecruitUI : WindowUI
{
    public EltHeroRecruit eltHeroRecruit;
    public Text textBtnRefresh;
    public Button btnRefresh;
    
    List<EltHeroRecruit> eltHeroRecruitList = new List<EltHeroRecruit>();
    TimeSpan remainTime;
    Coroutine co_RefreshTime;

    public void Start()
    {
        Show();
    }

    public override void Show()
    {
        base.Show();
        if (co_RefreshTime != null)
            StopCoroutine(co_RefreshTime);

        Set();
    }

    public void Set()
    {
        var list = Singleton.gm.gameData.infoHeroRecruits;
        for(int i = 0; i < list.Count; i++)
        {
            var elt = Utilities.GetOrCreate(eltHeroRecruitList, i, eltHeroRecruit.gameObject);
            elt.Set(list[i], i);
            elt.SetActive(true);
        }
        Utilities.DeactivateSurplus(eltHeroRecruitList, eltHeroRecruitList.Count);

        remainTime = Singleton.gm.gameData.remainTime;
        co_RefreshTime = StartCoroutine(RefreshTime());
    }

    public void OnClickRefresh()
    {
        Singleton.gm.gameData.RefreshRecruitList();
        Set();
    }

    IEnumerator RefreshTime()
    {
        while (remainTime.TotalSeconds > 0)
        {
            textBtnRefresh.text = $"{remainTime.Minutes}:{remainTime.Seconds}";
            yield return new WaitForSeconds(1f);
            remainTime -= TimeSpan.FromSeconds(1);
            Singleton.gm.gameData.remainTime = remainTime;
        }

        btnRefresh.enabled = true;
    }
}
