using System.Collections;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public enum TutorialType
{
    Start,
    Build,
    Merge,
    Quest,
    Recruit,

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
        List<short> tutorialDialogList;
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

                resourceManager.dicTutorialStrs.TryGetValue(type, out tutorialDialogList);
                for(int i = 0; i < tutorialDialogList.Count; i++)
                {
                    DataDialog dataDialog = DataDialog.Get(tutorialDialogList[i]);
                    if (dataDialog == null)
                        continue;
                    textName.text = DataTextTag.FindText(dataDialog.tagName);

                    string text = DataTextTag.FindText(dataDialog.tagText);
                    textTyper.Play(text);
                    yield return new WaitUntil(() => textTyper.isTyping == false);
                    yield return new WaitForSeconds(Mathf.Max(1f, text.Length * 0.1f));
                }
                break;
            case TutorialType.Build:
                rtArrow.SetActive(true);
                rtArrow.transform.localPosition = mainSM.buildings[0].transform.localPosition;
                funcClick = () => mainSM.OnClickBuild(BuildingType.MergeTable);
                break;
            case TutorialType.Merge:
                resourceManager.dicTutorialStrs.TryGetValue(type, out tutorialDialogList);
                for (int i = 0; i < tutorialDialogList.Count; i++)
                {
                    DataDialog dataDialog = DataDialog.Get(tutorialDialogList[i]);
                    if (dataDialog == null)
                        continue;
                    textName.text = DataTextTag.FindText(dataDialog.tagName);

                    string text = DataTextTag.FindText(dataDialog.tagText);
                    textTyper.Play(text);
                    yield return new WaitUntil(() => textTyper.isTyping == false);
                    yield return new WaitForSeconds(Mathf.Max(1f, text.Length * 0.1f));
                }

                rtArrow.SetActive(true);
                rtArrow.transform.localPosition = mainSM.buildings[0].transform.localPosition;
                funcClick = () => mergeWindow.OnClickChest(MergeItemCategory.WeaponWarrior);
                StartCoroutine(MovementArrow());
                yield return new WaitUntil(() => isClicked == false);

                break;
        }
        yield return null;
    }

    bool isClicked = false;
    public void OnClick()
    {
        if (textTyper.isTyping)
        {
            textTyper.OnQuickTyping();
            return;
        }

        funcClick?.Invoke();
        isClicked = true;
    }

    IEnumerator MovementArrow()
    {
        float time = 0f;
        Vector2 originPos = rtArrow.position;
        Vector2 endPos = originPos + new Vector2(90f,0);

        while (true)
        {
            while(time < 1f)
            {
                rtArrow.position = Vector2.Lerp(originPos, endPos, time);
                time += Time.deltaTime;
            }

            while(time > 0f)
            {
                rtArrow.position = Vector2.Lerp(originPos, endPos, time);
                time -= Time.deltaTime;
            }
        }
    }
}
