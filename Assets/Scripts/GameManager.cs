using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton
{
    public GameData gameData;

    public void Awake()
    {
        if (gm != null)
        {
            Destroy(gameObject);
            return;
        }

        gm = this;
        DontDestroyOnLoad(gameObject);

        Set();
    }

    public void Set()
    {
        gameData = new GameData();
    }
}
