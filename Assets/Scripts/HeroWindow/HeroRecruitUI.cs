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
        Observer.onRefreshRecruit += SetMember;
    }

    public void OnDestroy()
    {
        Observer.onRefreshRecruit -= SetMember;
    }

    public override void Show()
    {
        base.Show();
        Set();
    }

    public void Set()
    {
        var list = Singleton.gm.gameData.infoHeroRecruits;
        if (list == null || list.Count == 0)
        {
            Singleton.gm.gameData.RefreshRecruitList();
            Set();
            return;
        }
        SetMember();
        remainTime = Singleton.gm.gameData.recruitRefreshRemainTime;
        if (co_RefreshTime != null)
            StopCoroutine(co_RefreshTime);
        co_RefreshTime = StartCoroutine(RefreshTime());
    }

    void SetMember()
    {
        var list = Singleton.gm.gameData.infoHeroRecruits;
        var isRecruited = Singleton.gm.gameData.isHeroRecruited;
        for (int i = 0; i < list.Count; i++)
        {
            var elt = Utilities.GetOrCreate(eltHeroRecruitList, i, eltHeroRecruit.gameObject);
            elt.Set(list[i], i, isRecruited[i]);
            elt.SetActive(true);
        }
        Utilities.DeactivateSurplus(eltHeroRecruitList, eltHeroRecruitList.Count);
    }

    public void OnClickRefresh()
    {
        Singleton.gm.gameData.RefreshRecruitList();
        Set();
    }

    IEnumerator RefreshTime()
    {
        //btnRefresh.enabled = false;
        while (remainTime.TotalSeconds > 0)
        {
            textBtnRefresh.text = $"{remainTime.Minutes}:{remainTime.Seconds}";
            yield return new WaitForSeconds(1f);
            remainTime -= TimeSpan.FromSeconds(1);
            Singleton.gm.gameData.recruitRefreshRemainTime = remainTime;
        }

        btnRefresh.enabled = true;
    }
}
