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
    DataItem dataItem;

    public virtual void Set(int id, int idx)
    {
        this.idx = idx;
        this.id = id;

        dataItem = DataItem.Get(id);
        if (dataItem == null)
        {
            Debug.LogError($"Not Exist Data Item {id}");
            return;
        }
        
        if (imgIcon != null)
        {
            var sprite = Singleton.resourceManager.GetSprite(dataItem.resImage);
            if (sprite != null)
            {
                imgIcon.SetActive(true);
                imgIcon.sprite = sprite;
            }
            else
                imgIcon.SetActive(false);
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