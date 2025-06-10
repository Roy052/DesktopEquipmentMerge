using System.Collections.Generic;
using UnityEngine;

public class CollectableData : MonoBehaviour
{
    public static Dictionary<short, CollectableData> collectableDatas;

    public short id;

}

public class TempCollectableData
{
    public short id;
    
    public CollectableData Change()
    {
        return new CollectableData()
        {
            id = id,
        };
    }
}
