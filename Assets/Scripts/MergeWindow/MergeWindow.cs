using UnityEngine;
using System.Collections.Generic;

public class MergeWindow : GameWindow
{
    public GameObject objPrefab;
    public GameObject[,] panel;
    public EltMergeItem[,] eltMergeEqupments;

    public void Set(int[,] scraps)
    {
        int xLength = scraps.GetLength(0);
        int yLength = scraps.GetLength(1);
        panel = new GameObject[xLength, yLength];
        eltMergeEqupments = new EltMergeItem[xLength, yLength];
        for (int i = 0; i < scraps.GetLength(0); i++)
            for (int j = 0; j < scraps.GetLength(1); j++)
            {
                panel[i, j] = Instantiate(objPrefab, objPrefab.transform.parent);
                panel[i, j].SetActive(true);
                var temp = panel[i, j].GetComponent<EltMergeItem>();
                temp.Set(scraps[i, j], i * 100 + j);
                temp.funcClick = OnClick;
                if (temp != null)
                    eltMergeEqupments[i, j] = temp;
            }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            Singleton.gm.gameData.AddItems(new List<(short, long)>() { (0, 100) });
    }

    int idxFirst = -1;
    public void OnClick(int idx)
    {
        var coord = GetCoordByIdx(idx);
        var elt = eltMergeEqupments[coord.Item1, coord.Item2];
        if (elt == null)
            return;

        if (idxFirst == -1)
            idxFirst = idx;
        else
        {
            var coord1 = GetCoordByIdx(idxFirst);
            var elt1 = eltMergeEqupments[coord1.Item1, coord1.Item2];
            int type1st = elt1.GetScrapType();
            int type2nd = elt.GetScrapType();

            if (type1st == type2nd)
            {
                eltMergeEqupments[coord1.Item1, coord1.Item2].Set(0, idxFirst);
                eltMergeEqupments[coord.Item1, coord.Item2].Set(type1st + 1, idx);
            }

            idxFirst = -1;
        }
    }

    public void OnClickChest()
    {
        if (Singleton.gm.gameData.storedEquipmentList.Count == 0)
        {
            Debug.Log("Not Exist Stored Equipment");
            return;
        }

        List<(int, int)> emptyCoord = new List<(int, int)>();
        int xLength = eltMergeEqupments.GetLength(0);
        int yLength = eltMergeEqupments.GetLength(1);
        for (int i = 0; i < xLength; i++)
            for (int j = 0; j < yLength; j++)
            {
                if (eltMergeEqupments[i, j].GetScrapType() == 0)
                    emptyCoord.Add((i, j));
            }


        var randomCoord = emptyCoord[Random.Range(0, emptyCoord.Count)];
        int type = Singleton.gm.gameData.storedEquipmentList[0];
        Singleton.gm.gameData.storedEquipmentList.RemoveAt(0);
        eltMergeEqupments[randomCoord.Item1, randomCoord.Item2].Set(type, GetIdxByCoord(randomCoord));
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
