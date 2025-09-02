using UnityEngine;
using UnityEngine.UI;

public class ItemDescUI : MonoBehaviour
{
    public Image imgIcon;
    public Text textName;
    public Text textDesc;

    public GameObject objGet;
    int itemId = 0;
    int x = 0;
    int y = 0;

    public void Set(int itemId, int x, int y)
    {
        this.itemId = itemId;
        this.x = x;
        this.y = y;

        DataItem dataItem = DataItem.Get(itemId);
        if (dataItem == null)
        {
            Debug.LogError($"Not Exist Data Item : {itemId}");
            return;
        }

        var sprite = Singleton.resourceManager.GetSprite(dataItem.resImage);
        if (sprite == null)
        {
            Debug.Log($"Sprite Not Exst : {dataItem.resImage}");
            return;
        }

        imgIcon.sprite = sprite;
        textName.text = DataTextTag.FindText(dataItem.tagName);
        textDesc.text = DataTextTag.FindText(dataItem.tagDesc);
        objGet.SetActive(Utilities.CanStoreItem(itemId));
    }

    public void OnClickGet()
    {
        if (Utilities.CanStoreItem(itemId) == false)
            return;

        Singleton.gm.gameData.OnStoreMergeItem(x, y);
    }
}
