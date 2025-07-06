using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public Image imgItem;

    public void Set(string name)
    {
        imgItem.sprite = Singleton.resourceManager.dicResourceSprites[name];
    }
}
