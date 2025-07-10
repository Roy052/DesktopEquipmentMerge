using System.Collections;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum TutorialType
{
    Start,
}

public class TutorialManager : Singleton
{
    public Image[] imageStarts;

    public RectTransform rtArrow;
    public UnityAction funcClick;
    public GameObject objTextBox;
    public Text textName;
    public TextTyper textTyper;

    public void Awake()
    {
        tutorialManager = this;
    }

    public void OnDestroy()
    {
        tutorialManager = null;
    }

    public void Play(TutorialType type)
    {
        StartCoroutine(PlayTutorial(type));
    }

    public IEnumerator PlayTutorial(TutorialType type)
    {
        switch (type)
        {
            case TutorialType.Start:
                for(int i = 0; i < imageStarts.Length; i++)
                {
                    imageStarts[i].SetActive(true);
                    yield return StartCoroutine(FadeManager.FadeIn(imageStarts[i], 1f));
                }

                yield return new WaitForSeconds(1f);

                for (int i = 0; i < imageStarts.Length; i++)
                    imageStarts[i].SetActive(false);

                resourceManager.dicTutorialStrs.TryGetValue(type, out var list);
                for(int i = 0; i < list.Count; i++)
                {
                    DataDialog dataDialog = DataDialog.Get(list[i]);
                    if (dataDialog == null)
                        continue;
                    textName.text = DataTextTag.FindText(dataDialog.tagName);

                    string text = DataTextTag.FindText(dataDialog.tagText);
                    textTyper.Play(text);
                    yield return new WaitUntil(() => textTyper.isTyping == false);
                    yield return new WaitForSeconds(Mathf.Max(1f, text.Length * 0.1f));
                }

                rtArrow.SetActive(true);
                funcClick = () => mainSM.OnClickBuild(BuildingType.MergeTable);
                break;
        }
        yield return null;
    }

    public void OnClick()
    {
        if (textTyper.isTyping)
        {
            textTyper.OnQuickTyping();
            return;
        }

        funcClick?.Invoke();
    }
}
