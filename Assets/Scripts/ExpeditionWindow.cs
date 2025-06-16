using UnityEngine;

public class ExpeditionWindow : MonoBehaviour, IGameWindow
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
