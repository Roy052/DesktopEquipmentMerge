using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainBackground : Singleton
{
    public RectTransform rt;

    public void Start()
    {
        rt.offsetMin = new Vector2( rt.offsetMin.x, TaskbarInfo.taskBarHeight);
    }
}
