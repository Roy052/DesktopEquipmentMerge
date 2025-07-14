using UnityEngine;
using UnityEngine.Events;

public static class Observer
{
    public static UnityAction onRefreshExpedition;
    public static UnityAction onRefreshChest;
    public static UnityAction onRefreshBuilding;
    public static UnityAction onRefreshMergeWindow;
    public static UnityAction onRefreshHeroes;
    public static UnityAction onRefreshQuests;

    public static void RefreshAll()
    {
        onRefreshExpedition?.Invoke();
        onRefreshChest?.Invoke();
        onRefreshBuilding?.Invoke();
        onRefreshMergeWindow?.Invoke();
        onRefreshHeroes?.Invoke();
    }
}
