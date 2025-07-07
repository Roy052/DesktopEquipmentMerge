using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public Image imgItem;
    public Text textCount;

    public void Set(string name, int count)
    {
        if (Singleton.resourceManager.dicResourceSprites.TryGetValue(name, out var sprite))
            imgItem.sprite = sprite;
        textCount.text = count.ToString();
    }
}
