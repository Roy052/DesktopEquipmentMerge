using UnityEngine;

public class WindowUI : MonoBehaviour
{
    public bool IsShow => gameObject.activeSelf;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
