using System.Collections.Generic;
using UnityEngine;

public class ExpeditionMemberUI : WindowUI
{
    public EltHero eltHero;

    List<EltHero> eltHeroes = new List<EltHero>();
    HashSet<int> selectedMembers = new HashSet<int>();

    public override void Show()
    {
        base.Show();
        var list = Singleton.gm.gameData.infoHeroes;
        if (list == null)
        {
            Debug.LogError("No Member Exist");
            return;
        }

        for(int i = 0; i < list.Count; i++)
        {
            var elt = Utilities.GetOrCreate(eltHeroes, i, eltHero.gameObject);
            elt.Set(list[i]);
            elt.SetActive(true);
        }

        Utilities.DeactivateSurplus(eltHeroes, list.Count);
    }

    public void OnClickHero(int idx)
    {
        selectedMembers.Add(idx);
    }
}
