using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public Image imgItem;
    public Text textCount;

    public void Set(int id, long count)
    {
        DataItem dataItem = DataItem.Get(id);
        if(dataItem == null)
        {
            Debug.LogError($"Data Item Not Exist {id}");
            return;
        }

        var sprite = Singleton.resourceManager.GetSprite(dataItem.resImage);
        if (sprite != null)
            imgItem.sprite = sprite;
        if (textCount)
            textCount.text = Utilities.CountUnitConversion(count);
    }
}
