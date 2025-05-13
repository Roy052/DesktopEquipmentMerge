using UnityEngine;
using System.Collections.Generic;

public class MergeWindow : MonoBehaviour
{
    public GameObject objPrefab;
    public GameObject objEmpty;
    public GameObject[,] panel;
    public EltMergeScrap[,] eltMergeScraps;

    public void Set(int[,] scraps)
    {
        int xLength = scraps.GetLength(0);
        int yLength = scraps.GetLength(1);
        panel = new GameObject[xLength, yLength];
        eltMergeScraps = new EltMergeScrap[xLength, yLength];
        for (int i = 0; i < scraps.GetLength(0); i++)
            for (int j = 0; j < scraps.GetLength(1); j++)
            {
                panel[i, j] = Instantiate(objPrefab, objPrefab.transform.parent);
                panel[i, j].SetActive(true);
                var temp = panel[i, j].GetComponent<EltMergeScrap>();
                temp.Set(scraps[i, j], i * 100 + j);
                temp.funcClick = OnClick;
                if (temp != null)
                    eltMergeScraps[i, j] = temp;
            }
    }

    int idxFirst = -1;
    public void OnClick(int idx)
    {
        var coord = GetCoordByIdx(idx);
        var elt = eltMergeScraps[coord.Item1, coord.Item2];
        if (elt == null)
            return;

        if (idxFirst == -1)
            idxFirst = idx;
        else
        {
            var coord1 = GetCoordByIdx(idxFirst);
            var elt1 = eltMergeScraps[coord1.Item1, coord1.Item2];
            int type1st = elt1.GetScrapType();
            int type2nd = elt.GetScrapType();

            if (type1st == type2nd)
            {
                eltMergeScraps[coord.Item1, coord.Item2].Set(0, idx);
                eltMergeScraps[coord1.Item1, coord1.Item2].Set(type1st + 1, idxFirst);
            }

            idxFirst = -1;
        }
    }

    public void OnClickChest()
    {
        MainBackground cc = FindAnyObjectByType<MainBackground>();
        if (cc.storedScrapList.Count == 0)
        {
            Debug.Log("Not Exist Stored Scrap");
            return;
        }

        List<(int, int)> emptyCoord = new List<(int, int)>();
        int xLength = eltMergeScraps.GetLength(0);
        int yLength = eltMergeScraps.GetLength(1);
        for (int i = 0; i < xLength; i++)
            for (int j = 0; j < yLength; j++)
            {
                if (eltMergeScraps[i, j].GetScrapType() == 0)
                    emptyCoord.Add((i, j));
            }


        var randomCoord = emptyCoord[Random.Range(0, emptyCoord.Count)];
        int type = cc.storedScrapList[0];
        cc.storedScrapList.RemoveAt(0);
        eltMergeScraps[randomCoord.Item1, randomCoord.Item2].Set(type, GetIdxByCoord(randomCoord));
    }

    public (int, int) GetCoordByIdx(int idx)
    {
        return (idx / 100, idx % 100);
    }

    public int GetIdxByCoord((int, int) coord)
    {
        return coord.Item1 * 100 + coord.Item2;
    }
}
