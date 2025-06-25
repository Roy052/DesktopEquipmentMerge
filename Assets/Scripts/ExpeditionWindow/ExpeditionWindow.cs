using UnityEngine;

public class ExpeditionWindow : GameWindow
{
    public ExpeditionUI expeditionUI;
    public ExpeditionMemberUI expeditionMemberUI;

    public void Awake()
    {
        expeditionWindow = this;
    }

    public override void Show()
    {
        base.Show();
        expeditionUI.Show();
        expeditionMemberUI.Hide();
    }

    public void ShowExpeditionMember(DataExpedition data)
    {
        expeditionMemberUI.Show();
        expeditionMemberUI.Set(data);
    }

    public void HideExpeditionMember()
    {
        expeditionMemberUI.Hide();
    }
}
