using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static WaitForSeconds WaitForOneSecond = new WaitForSeconds(1);
    public static WaitForSeconds WaitForHalfSecond = new WaitForSeconds(0.5f);

    public static void SetActive(this Component c, bool active)
    {
        c.gameObject.SetActive(active);
    }
}
