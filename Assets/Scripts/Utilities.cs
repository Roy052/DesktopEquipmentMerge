using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    const int digit1 = 10000;
    const int digit2 = 100;

    public static WaitForSeconds WaitForOneSecond = new WaitForSeconds(1);
    public static WaitForSeconds WaitForHalfSecond = new WaitForSeconds(0.5f);

    public static void SetActive(this Component c, bool active)
    {
        c.gameObject.SetActive(active);
    }

    public static TElement GetOrCreate<TElement>(List<TElement> pool, int index, GameObject prefab)
    where TElement : Component          // EltTrader��EltQuest ��� Component �迭
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

        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }

    public static Vector2 GetLocalPosInCanvas(RectTransform rectTf, Canvas canvas)
    {
        // 1) ���� �� ��ũ�� ��ǥ
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rectTf.position);

        // 2) ��ũ�� �� ĵ���� ����(local) ��ǥ
        RectTransform cvRect = canvas.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cvRect, screen,
                                                                canvas.renderMode == RenderMode.ScreenSpaceOverlay
                                                                    ? null       // Overlay ���� ī�޶� �ʿ� ����
                                                                    : canvas.worldCamera,
                                                                out Vector2 local);
        return local;   // anchoredPosition �� ���� ����(+Y�� ��)
    }

    public static bool IsMergeItem(int itemId)
    {
        return itemId / digit1 == 1;
    }

    public static bool CanStoreItem(int itemId)
    {
        if (itemId / digit1 == 1)
        {
            MergeItemType mergeItemType = (MergeItemType)((itemId % digit1) / digit2);
            if (mergeItemType == MergeItemType.MoneyBag || mergeItemType == MergeItemType.ExpBook)
                return true;
        }
        else if (itemId / digit1 == 2)
            return true;

        return false;
    }
}
