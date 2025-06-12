using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainBackground : MonoBehaviour
{
    public RectTransform rt;
    public GameObject objHouse;
    public MergeWindow mergeWindow;

    public void Start()
    {
        rt.offsetMin = new Vector2( rt.offsetMin.x, TaskbarInfo.taskBarHeight);
        int[,] scraps = new int[3, 3];
        for (int i = 0; i < scraps.GetLength(0); i++)
            for (int j = 0; j < scraps.GetLength(1); j++)
                scraps[i, j] = Random.Range(0, 2);
        mergeWindow.Set(scraps);
    }

    public void OnClickOpenMergeWindow()
    {
        mergeWindow.gameObject.SetActive(!mergeWindow.gameObject.activeSelf);
    }
}
