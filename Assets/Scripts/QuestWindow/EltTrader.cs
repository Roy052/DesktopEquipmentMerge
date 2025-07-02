using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltTrader : MonoBehaviour
{
    public Image imgIcon;
    public Text textName;
    public Text textLv;

    public UnityAction<TraderType> funcClick;

    DataTrader dataTrader;

    public void Set(DataTrader data, short lv)
    {
        dataTrader = data;
        textName.text = DataTextTag.FindText(data.tagName);
        textLv.text = lv.ToString();
    }

    public void OnClick()
    {
        funcClick?.Invoke(dataTrader.type);
    }
}
