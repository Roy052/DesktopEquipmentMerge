using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CanvasChange : MonoBehaviour
{
    const float FallTime = 3f;
    const float RandomGap = 0.5f;

    public RectTransform rt;

    public GameObject objHouse;

    public MergeWindow mergeWindow;

    public EltFallingScrap fallingScrapPrefab;

    public List<int> storedScrapList = new List<int>();

    public void Start()
    {
#if !UNITY_EDITOR
        rt.offsetMin = new Vector2( rt.offsetMin.x, TaskbarInfo.taskBarHeight);
#endif
        int[,] scraps = new int[3, 3];
        for (int i = 0; i < scraps.GetLength(0); i++)
            for (int j = 0; j < scraps.GetLength(1); j++)
                scraps[i, j] = Random.Range(0, 2);
        mergeWindow.Set(scraps);
        currentTime = FallTime + Random.Range(-RandomGap, RandomGap);
    }

    float time = 0f;
    float currentTime = 0f;
    public void Update()
    {
        if (time >= currentTime)
        {
            time = 0f;
            currentTime = FallTime + Random.Range(-RandomGap, RandomGap);
            EltFallingScrap tempElt = Instantiate(fallingScrapPrefab, fallingScrapPrefab.transform.parent).GetComponent<EltFallingScrap>();
            tempElt.Set(1, 1);
            tempElt.OnFalling();
            tempElt.funcClick = (value) =>
            {
                storedScrapList.Add(value);
            };
        }
        time += Time.deltaTime;
    }

    public void OnClickOpenMergeWindow()
    {
        mergeWindow.gameObject.SetActive(!mergeWindow.gameObject.activeSelf);
    }
}
