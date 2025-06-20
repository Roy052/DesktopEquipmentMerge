using UnityEngine;
using UnityEngine.UI;

public class EltTrader : MonoBehaviour
{
    public Image imgIcon;
    public Text textName;
    public Text textLv;

    public void Set(DataTrader data, short lv)
    {
        textName.text = DataTextTag.FindText(data.tagName);
        textLv.text = lv.ToString();
    }
}
