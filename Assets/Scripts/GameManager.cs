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

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
            GameData.Save(gameData);
        if (Input.GetKeyUp(KeyCode.L))
        {
            gameData = GameData.Load();
            Observer.RefreshAll();
        }

    }

    public void Set()
    {
        gameData = new GameData();
        gameData.AddItems(new List<(int, long)>() { (0, 100) });
        StartCoroutine(RefreshExpedition());
        mainSM.Set();
    }

    IEnumerator RefreshExpedition()
    {
        while (true)
        {
            if (gameData == null)
                yield break;

            gameData.RefreshExpedition();
            gameData.RefreshInjury();
            yield return new WaitForSeconds(1f);
        }
    }
}
