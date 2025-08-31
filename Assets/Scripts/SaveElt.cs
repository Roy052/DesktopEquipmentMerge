using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SaveElt : MonoBehaviour
{
    public Text textTitle;
    public UnityAction<int> funcClick;

    int idx;

    public void Set(string title, int idx)
    {
        this.idx = idx;
        textTitle.text = title;
    }

    public void OnClickLoad()
    {
        funcClick?.Invoke(idx);
    }
}
