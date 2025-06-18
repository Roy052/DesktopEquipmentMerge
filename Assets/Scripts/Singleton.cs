using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static GameManager gm;
    public static ResourceManager resourceManager;
    public static DataManager dataManager;

    //Window
    public static MergeWindow mergeWindow;
    public static HeroWindow heroExpeditionWindow;
    public static QuestWindow questWindow;
}
