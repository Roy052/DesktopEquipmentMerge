using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltQuestProgress : MonoBehaviour
{
    const float MaxWidth = 100f;

    public Text textRequireItem;
    public Text textProgress;

    public RectTransform rectProgressBar;
    public GameObject objDisable; 
    public GameObject objBtnSubmit;

    public UnityAction<int> funcSubmit;
    int idx = -1;

    public void Set(int idx, int itemId, int currentCount, int requireCount)
    {
        this.idx = idx;

        DataItem dataItem = DataItem.Get((short)itemId);
        if (dataItem == null)
            return;

        textRequireItem.text = DataTextTag.FindText(dataItem.tagName);
        textProgress.text = $"{currentCount} / {requireCount}";
        rectProgressBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MaxWidth * ((float)currentCount / requireCount));
        objDisable.SetActive(currentCount >= requireCount);
        objBtnSubmit.SetActive(currentCount < requireCount);
    }

    public void OnClickSubmit()
    {
        funcSubmit?.Invoke(idx);
    }
}
