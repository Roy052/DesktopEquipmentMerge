using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static WaitForSeconds WaitForOneSecond = new WaitForSeconds(1);
    public static WaitForSeconds WaitForHalfSecond = new WaitForSeconds(0.5f);

    public static void SetActive(this Component c, bool active)
    {
        c.gameObject.SetActive(active);
    }

    public static TElement GetOrCreate<TElement>(List<TElement> pool, int index, GameObject prefab)
    where TElement : Component          // EltTrader·EltQuest 모두 Component 계열
    {
        if (pool.Count <= index)
        {
            var go = MonoBehaviour.Instantiate(prefab, prefab.transform.parent);
            var elt = go.GetComponent<TElement>();
            pool.Add(elt);
            return elt;
        }
        return pool[index];
    }
    
    public static void DeactivateSurplus<TElement>(List<TElement> pool, int from)
    where TElement : MonoBehaviour
    {
        for (int i = from; i < pool.Count; i++)
            pool[i].gameObject.SetActive(false);
    }

    static long[] ConvValue = new long[] { 1000000, 1000, };
    public static string CountUnitConversion(long count)
    {
        if (count >= ConvValue[0] * 100)
            return $"{count / ConvValue[0]}M";
        else if (count >= ConvValue[1] * 100)
            return $"{count / ConvValue[1]}K";
        else
            return $"{count}";
    }

    public static string GetStringRemainSec(int remainSec)
    {
        int hours = remainSec / 3600;
        int minutes = (remainSec % 3600) / 60;
        int seconds = remainSec % 60;

        if (hours > 0)
            return $"{hours:00}:{minutes:00}:{seconds:00}";
        else if (minutes > 0)
            return $"{minutes:00}:{seconds:00}";
        else
            return $"{seconds:00}";
    }
}
