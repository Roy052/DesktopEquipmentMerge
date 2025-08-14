using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MsgBox : Singleton
{
    public Text textTitle;
    public Text textMessage;

    public GameObject objYes;
    public GameObject objNo;
    public GameObject objConfirm;

    UnityAction funcYes;
    UnityAction funcConfirm;

    public void Awake()
    {
        msgBox = this;
    }

    public void OnDestroy()
    {
        msgBox = null;
    }

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetYesNo(string title, string msg, UnityAction funcYes = null)
    {
        this.funcYes = funcYes;

        textTitle.text = title;
        textMessage.text = msg;
    }

    public void SetConfirm(string title, string msg, UnityAction funcConfirm = null)
    {
        this.funcConfirm = funcConfirm;

        textTitle.text = title;
        textMessage.text = msg;
    }

    public void OnClickYes()
    {
        funcYes?.Invoke();
        gameObject.SetActive(false);
    }

    public void OnClickNo()
    {
        gameObject.SetActive(false);
    }

    public void OnClickConfirm()
    {
        funcConfirm?.Invoke();
        gameObject.SetActive(false);
    }
}
