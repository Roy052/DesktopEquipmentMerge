using UnityEngine;
using System.Collections.Generic;

public class MergeWindow : GameWindow
{
    [Header("Chest")]
    public EltChest eltChest;
    List<EltChest> eltChestList = new();

    [Header("StoneTable")]
    public GameObject objStone;
    List<GameObject> stoneList = new();

    [Header("MergeTable")]
    public GameObject objEltItem;
    public GameObject[,] panel;
    public EltMergeItem[,] eltMergeItems;

    [Header("ItemDescUI")]
    public ItemDescUI itemDescUI;

    protected override void Awake()
    {
        base.Awake();
        mergeWindow = this;
        Observer.onRefreshMergeWindow += RefreshMergeWindow;
        Observer.onRefreshChest += RefreshChest;
        eltChest.SetActive(false);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        mergeWindow = null;
        Observer.onRefreshMergeWindow -= RefreshMergeWindow;
        Observer.onRefreshChest -= RefreshChest;
    }

    public override void Show()
    {
        base.Show();
        RefreshMergeWindow();
    }

    public void RefreshMergeWindow()
    {
        mergeWindow.Set(gm.gameData.mergeItems);
    }

    public void Set(int[,] mergeItems)
    {
        //DEMO
        for(int i = 0; i < (int)MergeItemCategory.WeaponArcher + 1; i++)
        {
            var elt = Utilities.GetOrCreate(eltChestList, i, eltChest.gameObject);
            elt.Set((MergeItemCategory)i);
            elt.funcClick = OnClickChest;
            elt.SetActive(true);
        }

        Utilities.DeactivateSurplus(eltChestList, (int)MergeItemCategory.Max);

        int row = mergeItems.GetLength(0);
        int col = mergeItems.GetLength(1);

        for (int i = 0; i < row * col; i++)
            if (stoneList.Count <= i)
            {
                GameObject temp = Instantiate(objStone, objStone.transform.parent);
                temp.SetActive(true);
                stoneList.Add(temp);
            }

        if (panel == null)
        {
            panel = new GameObject[row, col];
            eltMergeItems = new EltMergeItem[row, col];
        }
        else if(panel.GetLength(0) != row)
        {
            GameObject[,] tempPanel = new GameObject[row, col];
            EltMergeItem[,] tempElt = new EltMergeItem[row, col];

            int tempRow = panel.GetLength(0);
            int tempCol = panel.GetLength(1);
            for(int i = 0; i < tempRow; i++)
                for(int j = 0; j < tempCol; j++)
                {
                    tempPanel[i, j] = panel[i, j];
                    tempElt[i, j] = eltMergeItems[i, j];
                }

            panel = tempPanel;
            eltMergeItems = tempElt;
        }
            
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                EltMergeItem temp = null;
                if (panel[i, j] == null)
                {
                    panel[i, j] = Instantiate(objEltItem, objEltItem.transform.parent);
                    panel[i, j].SetActive(true);
                    temp = panel[i, j].GetComponent<EltMergeItem>();
                    temp.funcMerge = OnMergeSelect;
                    temp.funcClick = OnShowItemDescUI;
                }
                else
                    temp = eltMergeItems[i, j];

                temp.Set(mergeItems[i, j], i * 100 + j);
                if (temp != null)
                    eltMergeItems[i, j] = temp;
            }
    }

    int idxFirst = -1;
    public void OnMergeSelect(int idx)
    {
        var coord = GetCoordByIdx(idx);
        var elt = eltMergeItems[coord.Item1, coord.Item2];
        if (elt == null)
            return;

        if (idxFirst == -1)
            idxFirst = idx;
        else
        {
            var coord1 = GetCoordByIdx(idxFirst);
            gm.gameData.OnMergeItems(coord1, coord);
            idxFirst = -1;
        }
    }

    public void OnShowItemDescUI(int idx)
    {
        var coord = GetCoordByIdx(idx);
        var elt = eltMergeItems[coord.Item1, coord.Item2];
        if (elt == null)
            return;

        itemDescUI.SetActive(true);
        itemDescUI.Set(elt.GetMergeItemId(), coord.Item1, coord.Item2);
    }

    public void OnClickChest(MergeItemCategory category)
    {
        gm.gameData.OnChestToAddMergeItem(category);
    }

    public (int, int) GetCoordByIdx(int idx)
    {
        return (idx / 100, idx % 100);
    }

    public int GetIdxByCoord((int, int) coord)
    {
        return coord.Item1 * 100 + coord.Item2;
    }

    void RefreshChest()
    {
        //DEMO
        for (int i = 0; i < (int)MergeItemCategory.WeaponArcher + 1; i++)
            if(eltChestList.Count > i)
                eltChestList[i].Refresh();
    }
}
