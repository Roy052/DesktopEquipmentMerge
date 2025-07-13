using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public enum TutorialType
{
    Start,
    Enter,
    PullEquipment,
    Merge,
    Quest,
    Recruit,
    Max,
}

public class TutorialManager : Singleton
{
    const float PosArrowY = 30f;

    public Image[] imageStarts;

    public RectTransform rtArrow;
    public UnityAction funcClick;
    public GameObject objTextBox;
    public Text textName;
    public TextTyper textTyper;
    public GameObject objClick;

    Coroutine co_MovementArrow;

    public void Awake()
    {
        tutorialManager = this;
        rtArrow.SetActive(false);
        objTextBox.SetActive(false);
    }

    public void OnDestroy()
    {
        tutorialManager = null;
    }

    private void Update()
    {
        for(int i = 0; i < (int)TutorialType.Recruit + 1; i++)
        {

            if (Input.GetKeyUp(KeyCode.Alpha1 + i))
            {
                Play(TutorialType.Start + i);
            }
        }
    }

    public void Play(TutorialType type)
    {
        StartCoroutine(PlayTutorial(type));
    }

    public IEnumerator PlayTutorial(TutorialType type)
    {
        isClicked = false;
        objClick.SetActive(true);

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
                yield return StartCoroutine(PlayDialog(tutorialDialogList));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[0].transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());
                funcClick = () => mainSM.OnClickBuild(BuildingType.MergeTable);
                yield return new WaitUntil(() => isClicked);
                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.Enter:
                resourceManager.dicTutorialStrs.TryGetValue(type, out tutorialDialogList);
                yield return StartCoroutine(PlayDialog(tutorialDialogList));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[0].transform as RectTransform, mainCanvas) + new Vector2(0, 90f);
                co_MovementArrow = StartCoroutine(MovementArrow());
                funcClick = () => mainSM.buildings[0].OnClick();
                yield return new WaitUntil(() => isClicked);
                if(co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.PullEquipment:
                resourceManager.dicTutorialStrs.TryGetValue(type, out tutorialDialogList);
                yield return StartCoroutine(PlayDialog(tutorialDialogList));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mergeWindow.eltChest.transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());
                funcClick = () => mergeWindow.OnClickChest(MergeItemCategory.WeaponWarrior);
                yield return new WaitUntil(() => isClicked);
                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.Merge:
                resourceManager.dicTutorialStrs.TryGetValue(type, out tutorialDialogList);
                yield return StartCoroutine(PlayDialog(tutorialDialogList));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                Vector2 originPos = Vector2.zero;
                Vector2 endPos = Vector2.zero;

                int row = gm.gameData.row;
                int col = gm.gameData.col;
                for(int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        if (mergeWindow.eltMergeItems[i, j].GetMergeItemId() == -1)
                            continue;

                        if (originPos == Vector2.zero)
                            originPos = Utilities.GetLocalPosInCanvas(mergeWindow.eltMergeItems[i, j].transform as RectTransform, mainCanvas);
                        else
                            endPos = Utilities.GetLocalPosInCanvas(mergeWindow.eltMergeItems[i, j].transform as RectTransform, mainCanvas);
                    }
                }

                objClick.SetActive(false);
                co_MovementArrow = StartCoroutine(MovementArrow(originPos, endPos));
                Observer.onRefreshMergeWindow += () => isClicked = true;
                yield return new WaitUntil(() => isClicked);
                Observer.onRefreshMergeWindow -= () => isClicked = true;
                StopCoroutine(co_MovementArrow);
                break;
        }
        yield return null;

        rtArrow.SetActive(false);
        objTextBox.SetActive(false);
        isClicked = false;
        objClick.SetActive(false);
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
        Vector2 originPos = rtArrow.localPosition;
        Vector2 endPos = originPos + new Vector2(0, PosArrowY);

        while (true)
        {
            while(time < 1f)
            {
                rtArrow.localPosition = Vector2.Lerp(originPos, endPos, time);
                time += Time.deltaTime * 2f;
                yield return null;
                Debug.Log($"{rtArrow.localPosition} : {time}");
            }

            while(time > 0f)
            {
                rtArrow.localPosition = Vector2.Lerp(originPos, endPos, time);
                time -= Time.deltaTime * 2f;
                yield return null;
                Debug.Log($"{rtArrow.localPosition} : {time}");
            }
        }
    }

    IEnumerator MovementArrow(Vector2 originPos, Vector2 endPos)
    {
        float time = 0f;

        while (true)
        {
            while (time < 1f)
            {
                rtArrow.localPosition = Vector2.Lerp(originPos, endPos, time);
                time += Time.deltaTime * 2f;
                yield return null;
                Debug.Log($"{rtArrow.localPosition} : {time}");
            }

            while (time > 0f)
            {
                rtArrow.localPosition = Vector2.Lerp(originPos, endPos, time);
                time -= Time.deltaTime * 2f;
                yield return null;
                Debug.Log($"{rtArrow.localPosition} : {time}");
            }
        }
    }

    IEnumerator PlayDialog(List<short> tutorialDialogList)
    {
        objTextBox.SetActive(true);

        for (int i = 0; i < tutorialDialogList.Count; i++)
        {
            isClicked = false;
            DataDialog dataDialog = DataDialog.Get(tutorialDialogList[i]);
            if (dataDialog == null)
                continue;
            textName.text = DataTextTag.FindText(dataDialog.tagName);

            string text = DataTextTag.FindText(dataDialog.tagText);
            textTyper.Play(text);
            yield return new WaitUntil(() => textTyper.isTyping == false);
            yield return new WaitForSeconds(0.3f);
            yield return new WaitUntil(() => isClicked);
        }
    }
}
