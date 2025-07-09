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
    int idx = -1;

    public void Set(int id, int idx)
    {
        this.idx = idx;
        this.id = id;
        if (imgIcon != null)
        {
            Singleton.resourceManager.dicEquipmentSprites.TryGetValue(id, out var sprite);
            imgIcon.sprite = sprite;
            imgIcon.color = this.id == 1 ? Color.red : Color.yellow;
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