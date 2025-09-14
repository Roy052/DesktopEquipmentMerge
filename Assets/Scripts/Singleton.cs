using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static GameManager gm;
    public static ResourceManager resourceManager;
    public static DataManager dataManager;
    public static MainBackground mainBackground;
    public static SoundManager soundManager;
    public static TutorialManager tutorialManager;

    public static MainSM mainSM;
    public static Canvas mainCanvas;

    //Window
    public static MergeWindow mergeWindow;
    public static HeroWindow heroWindow;
    public static ExpeditionWindow expeditionWindow;
    public static QuestWindow questWindow;
    public static StorageWindow storageWindow;

    //Etc
    public static MsgBox msgBox;
}
