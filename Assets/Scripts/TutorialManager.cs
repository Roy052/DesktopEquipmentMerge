using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public enum TutorialType
{
    Start,
    EnterMerge,
    PullEquipment,
    Merge,
    Quest,
    BuildOthers,
    EnterInn,
    Recruit,
    Expedition,
    ExpeditionMember,
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

                objClick.SetActive(false);
                Observer.onRefreshBuilding += ChangeIsClicked;
                yield return new WaitUntil(() => isClicked);
                Observer.onRefreshBuilding -= ChangeIsClicked;

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.EnterMerge:
                resourceManager.dicTutorialStrs.TryGetValue(type, out tutorialDialogList);
                yield return StartCoroutine(PlayDialog(tutorialDialogList));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[0].transform as RectTransform, mainCanvas) + new Vector2(0, 90f);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => mergeWindow.IsShow);

                if (co_MovementArrow != null)
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

                objClick.SetActive(false);
                yield return new WaitUntil(() => gm.gameData.GetCountMergeItemId(10001) >= 2);

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
                co_MovementArrow = StartCoroutine(MovementArrow(originPos, endPos));

                objClick.SetActive(false);
                Observer.onRefreshMergeWindow += ChangeIsClicked;
                yield return new WaitUntil(() => isClicked);
                Observer.onRefreshMergeWindow -= ChangeIsClicked;

                StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.BuildOthers:
                resourceManager.dicTutorialStrs.TryGetValue(type, out tutorialDialogList);
                yield return StartCoroutine(PlayDialog(tutorialDialogList));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[(int)BuildingType.BaseCamp].transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => gm.gameData.buildingLvs[(int)BuildingType.BaseCamp] >= 1);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[(int)BuildingType.Inn].transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => gm.gameData.buildingLvs[(int)BuildingType.Inn] >= 1);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.EnterInn:
                resourceManager.dicTutorialStrs.TryGetValue(type, out tutorialDialogList);
                yield return StartCoroutine(PlayDialog(tutorialDialogList));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[(int)BuildingType.Inn].transform as RectTransform, mainCanvas) + new Vector2(0, 90f);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => heroWindow.IsShow);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.Recruit:
                resourceManager.dicTutorialStrs.TryGetValue(type, out tutorialDialogList);
                yield return StartCoroutine(PlayDialog(tutorialDialogList));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                RectTransform rtRecruit = (heroWindow.uis[(int)HeroUI.HeroRecruit] as HeroRecruitUI).eltHeroRecruit.transform.parent as RectTransform;
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(rtRecruit, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                Observer.onRefreshHeroes += ChangeIsClicked;
                yield return new WaitUntil(() => isClicked);
                Observer.onRefreshHeroes -= ChangeIsClicked;

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.Expedition:
                resourceManager.dicTutorialStrs.TryGetValue(type, out tutorialDialogList);
                yield return StartCoroutine(PlayDialog(tutorialDialogList));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                RectTransform rtExpedition = expeditionWindow.expeditionUI.eltExpedition.btnSelectExpeditionMember.transform as RectTransform;
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(rtExpedition, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => expeditionWindow.expeditionMemberUI.IsShow);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.ExpeditionMember:
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

    IEnumerator MovementArrowOneSide(Vector2 originPos, Vector2 endPos)
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

            rtArrow.localPosition = originPos;
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

    void ChangeIsClicked()
    {
        isClicked = true;
    }
}
