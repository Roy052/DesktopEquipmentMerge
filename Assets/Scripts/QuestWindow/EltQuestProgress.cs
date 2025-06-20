using UnityEngine;
using UnityEngine.UI;

public class EltQuestProgress : MonoBehaviour
{
    const float MaxWidth = 100f;

    public Text textRequireItem;
    public Text textProgress;

    public RectTransform rectProgressBar;
    public GameObject objDisable;

    public void Set(int itemId, int currentCount, int requireCount)
    {
        DataItem dataItem = DataItem.Get((short)itemId);
        if (dataItem == null)
            return;

        textRequireItem.text = DataTextTag.FindText(dataItem.tagName);
        textProgress.text = $"{currentCount} / {requireCount}";
        rectProgressBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MaxWidth * ((float)currentCount / requireCount));
        objDisable.SetActive(currentCount >= requireCount);
    }
}
