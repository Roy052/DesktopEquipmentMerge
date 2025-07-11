using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltItem : MonoBehaviour
{
    public Image imgIcon;
    public UnityAction<int> funcClick;
    public GameObject objSelect;

    int id = -1;
    int grade = -1;
    protected int idx = -1;

    public void Set(int id, int idx)
    {
        this.idx = idx;
        this.id = id;
        if (imgIcon != null)
        {
            DataItem dataItem = DataItem.Get(id);
            if (dataItem != null && Singleton.resourceManager.dicResourceSprites.TryGetValue(dataItem.resImage, out var sprite))
                imgIcon.sprite = sprite;
        }
    }

    public virtual void OnClick()
    {
        funcClick?.Invoke(idx);
    }

    public int GetMergeItemId()
    {
        return id;
    }
}