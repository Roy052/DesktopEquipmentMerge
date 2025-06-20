using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltItem : MonoBehaviour
{
    public Image imgIcon;
    public UnityAction<int> funcClick;
    public GameObject objSelect;

    int type = -1;
    int grade = -1;
    int idx = -1;

    public void Set(int typeIdx, int idx)
    {
        this.idx = idx;

        type = typeIdx;
        if (typeIdx == 0)
            imgIcon.sprite = null;
        else
            imgIcon.sprite = Singleton.resourceManager.dicEquipmentSprites[typeIdx];
        imgIcon.color = type == 1 ? Color.red : Color.yellow;
    }

    public virtual void OnClick()
    {
        funcClick?.Invoke(idx);
    }

    public int GetScrapType()
    {
        return type;
    }
}