using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton
{
    public TitleUI titleUI;
    public GameObject objMain;
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
        titleUI.SetActive(true);
        titleUI.Open();
        objMain.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            titleUI.SetActive(!titleUI.gameObject.activeSelf);
            if (titleUI.gameObject.activeSelf)
                titleUI.Open();
        }
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

    public void OnClickPlay()
    {
        gameData = new GameData();
        gameData.AddItems(new List<(int, long)>() { (0, 100) });
        StartCoroutine(RefreshExpedition());
        mainSM.Set();
        GameData.Save(gameData);

        titleUI.SetActive(false);
        objMain.SetActive(true);
    }

    public void OnSave()
    {
        if (gameData == null)
            return;

        GameData.Save(gameData);
    }
}
