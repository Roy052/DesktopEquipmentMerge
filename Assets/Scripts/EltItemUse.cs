using UnityEngine;
using UnityEngine.UI;

public class EltItemUse : EltItem
{
    public Text textUseCount;
    public Text textMaxCount;

    long count = 0;
    long maxCount = 0;

    public override void Set(int id, int idx)
    {
        base.Set(id, idx);
        Singleton.gm.gameData.itemCounts.TryGetValue(id, out long count);
        maxCount = count;
        textMaxCount.text = $"/{count}";
    }

    public override void OnClick()
    {
        if (count >= maxCount)
            return;

        base.OnClick();
    }

    public void OnClickRemove()
    {
        if (count <= 0)
            return;

        count--;
    }
}
