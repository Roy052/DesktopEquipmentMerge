using System.Collections;
using System.Collections.Generic;
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
    }

    public void Start()
    {
        Set();
    }

    public void Set()
    {
        gameData = new GameData();
        gameData.AddItems(new List<(short, long)>() { (0, 100) });
        StartCoroutine(RefreshExpedition());
        mainSM.Set();
    }

    IEnumerator RefreshExpedition()
    {
        while (true)
        {
            Singleton.gm.gameData.RefreshExpedition();
            yield return new WaitForSeconds(1f);
        }
    }
}
