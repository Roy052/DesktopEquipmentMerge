using UnityEngine;
using UnityEngine.Events;

public static class Observer
{
    public static UnityAction onRefreshRecruit;
    public static UnityAction onRefreshExpedition;
    public static UnityAction onRefreshChest;
    public static UnityAction onRefreshBuilding;
    public static UnityAction onRefreshMergeWindow;
    public static UnityAction onRefreshHeroes;

    public static void RefreshAll()
    {
        onRefreshRecruit?.Invoke();
        onRefreshExpedition?.Invoke();
        onRefreshChest?.Invoke();
        onRefreshBuilding?.Invoke();
        onRefreshMergeWindow?.Invoke();
        onRefreshHeroes?.Invoke();
    }
}
