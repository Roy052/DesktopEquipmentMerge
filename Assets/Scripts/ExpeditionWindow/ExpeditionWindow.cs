using UnityEngine;

public class ExpeditionWindow : GameWindow
{
    public ExpeditionUI expeditionUI;
    public ExpeditionMemberUI expeditionMemberUI;

    public override void Show()
    {
        base.Show();
        expeditionUI.Show();
        expeditionMemberUI.Hide();
    }
}
