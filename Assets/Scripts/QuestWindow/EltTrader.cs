using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltTrader : MonoBehaviour
{
    public Image imgIcon;
    public Text textName;
    public Text textLv;
    public GameObject objSelected;

    public UnityAction<TraderType> funcClick;

    DataTrader dataTrader;

    public void Set(DataTrader data, short lv)
    {
        if (data != null && Singleton.resourceManager.dicResourceSprites.TryGetValue(data.resImage, out var sprite))
        {
            imgIcon.SetActive(true);
            imgIcon.sprite = sprite;
        }
        dataTrader = data;
        textName.text = DataTextTag.FindText(data.tagName);
        textLv.text = lv.ToString();
    }

    public void OnClick()
    {
        funcClick?.Invoke(dataTrader.type);
    }

    public TraderType TraderType => dataTrader.type;
}
