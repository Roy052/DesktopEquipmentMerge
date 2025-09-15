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
    BuildQuest,
    EnterQuest,
    ClickTrader,
    ClickQuest,
    ClickAccept,
    ClickSubmit,
    ClickReward,
    BuildHeroInn,
    EnterHeroInn,
    Recruit,
    BuildExpedition,
    EnterExpedition,
    Expedition,
    ExpeditionMember,
    ClickExpedition,
    ClickExpeditionReward,
    End,
    Max,
}

public class TutorialManager : Singleton
{
    const float PosArrowY = 30f;
    readonly Vector2 PosUpperBuilding = new Vector2(0, 90f);

    public Image[] imageStarts;

    public RectTransform rtArrow;
    public UnityAction funcClick;
    public GameObject objTextBox;
    public Text textName;
    public TextTyper textTyper;
    public GameObject objClick;

    Coroutine co_MovementArrow;
    Coroutine co_CurrentTutorial;

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
        if (co_CurrentTutorial != null)
            return;

        if (gm.gameData == null)
            return;

        bool isAllCleared = true;
        for (int i = 0; i < (int)TutorialType.Max; i++)
        {
            if (gm.gameData.isTutorialShowed[i] == false)
            {
                switch ((TutorialType)i)
                {
                    case TutorialType.ClickExpeditionReward:
                        if (gm.gameData.dictInfoExpeditions.Count == 0 || 
                            gm.gameData.dictInfoExpeditions.ContainsKey(1) == false ||
                            gm.gameData.dictInfoExpeditions[1].state != ExpeditionState.Reward)
                            continue;
                        break;
                }

                Play((TutorialType)i);
                isAllCleared = false;
                break;
            }
        }

        if (isAllCleared)
            Destroy(gameObject);
    }

    public void Play(TutorialType type)
    {
        if (co_CurrentTutorial != null)
            return;

        co_CurrentTutorial = StartCoroutine(PlayTutorial(type));
    }

    public IEnumerator PlayTutorial(TutorialType type)
    {
        isClicked = false;
        objClick.SetActive(true);

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

                
                yield return StartCoroutine(PlayDialog(type));
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
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[0].transform as RectTransform, mainCanvas) + PosUpperBuilding;
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => mergeWindow);
                yield return new WaitUntil(() => mergeWindow.IsShow);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.PullEquipment:
                yield return StartCoroutine(PlayDialog(type));
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
                yield return StartCoroutine(PlayDialog(type));
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
            case TutorialType.BuildQuest:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[(int)BuildingType.Traderhall].transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                Observer.onRefreshBuilding += ChangeIsClicked;
                yield return new WaitUntil(() => isClicked);
                Observer.onRefreshBuilding -= ChangeIsClicked;

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.EnterQuest:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[(int)BuildingType.Traderhall].transform as RectTransform, mainCanvas) + PosUpperBuilding;
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => questWindow);
                yield return new WaitUntil(() => questWindow.IsShow);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.ClickTrader:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(questWindow.eltTrader.transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => questWindow);
                yield return new WaitUntil(() => questWindow.GetTraderType() != TraderType.None);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.ClickQuest:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                yield return new WaitUntil(() => questWindow);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(questWindow.eltQuest.transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => questWindow);
                yield return new WaitUntil(() => questWindow.GetInfoQuest() != null);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.ClickAccept:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(questWindow.objBtnAccept.transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => questWindow);
                yield return new WaitUntil(() => questWindow.GetInfoQuest().state == QuestState.Progress);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.ClickSubmit:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(questWindow.eltQuestProgress.objBtnSubmit.transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => questWindow);
                yield return new WaitUntil(() => questWindow.GetInfoQuest().state == QuestState.Reward);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.ClickReward:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(questWindow.objBtnReward.transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => questWindow);
                yield return new WaitUntil(() => questWindow.GetInfoQuest().state == QuestState.Clear);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.BuildHeroInn:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[(int)BuildingType.HeroInn].transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => gm.gameData.buildingLvs[(int)BuildingType.HeroInn] >= 1);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.EnterHeroInn:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[(int)BuildingType.HeroInn].transform as RectTransform, mainCanvas) + PosUpperBuilding;
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => heroWindow);
                yield return new WaitUntil(() => heroWindow.IsShow);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.Recruit:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                objClick.SetActive(false);
                yield return new WaitUntil(() => heroWindow);
                RectTransform rtRecruit = (heroWindow.uis[(int)HeroUI.HeroRecruit] as HeroRecruitUI).eltHeroRecruit.transform.parent as RectTransform;
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(rtRecruit, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => gm.gameData.infoHeroes.Count > 0);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.BuildExpedition:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[(int)BuildingType.ExpeditionCamp].transform as RectTransform, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => gm.gameData.buildingLvs[(int)BuildingType.ExpeditionCamp] >= 1);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.EnterExpedition:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(mainSM.buildings[(int)BuildingType.ExpeditionCamp].transform as RectTransform, mainCanvas) + PosUpperBuilding;
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => expeditionWindow);
                yield return new WaitUntil(() => expeditionWindow.IsShow);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.Expedition:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                objClick.SetActive(false);
                yield return new WaitUntil(() => expeditionWindow);
                isClicked = false;
                rtArrow.SetActive(true);
                RectTransform rtExpedition = expeditionWindow.expeditionUI.eltExpedition.btnSelectExpeditionMember.transform as RectTransform;
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(rtExpedition, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => expeditionWindow.expeditionMemberUI);
                yield return new WaitUntil(() => expeditionWindow.expeditionMemberUI.IsShow);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.ExpeditionMember:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                objClick.SetActive(false);
                yield return new WaitUntil(() => expeditionWindow);
                yield return new WaitUntil(() => expeditionWindow.expeditionMemberUI);
                isClicked = false;
                rtArrow.SetActive(true);
                RectTransform rtExpeditionHero = expeditionWindow.expeditionMemberUI.eltHero.transform as RectTransform;
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(rtExpeditionHero, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => expeditionWindow.expeditionMemberUI.IsEmptyMember() == false);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.ClickExpedition:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                RectTransform rtBtnExpedition = expeditionWindow.expeditionMemberUI.btnGoExpedition.transform as RectTransform;
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(rtBtnExpedition, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => gm.gameData.dictInfoExpeditions.ContainsKey(1));

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.ClickExpeditionReward:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                objTextBox.SetActive(false);

                isClicked = false;
                rtArrow.SetActive(true);
                RectTransform rtBtnExpeditionReward = expeditionWindow.expeditionUI.eltExpedition.btnGetReward.transform as RectTransform;
                rtArrow.localPosition = Utilities.GetLocalPosInCanvas(rtBtnExpeditionReward, mainCanvas);
                co_MovementArrow = StartCoroutine(MovementArrow());

                objClick.SetActive(false);
                yield return new WaitUntil(() => gm.gameData.dictInfoExpeditions.ContainsKey(1) == false);

                if (co_MovementArrow != null)
                    StopCoroutine(co_MovementArrow);
                break;
            case TutorialType.End:
                yield return StartCoroutine(PlayDialog(type));
                yield return new WaitUntil(() => isClicked);
                break;
        }
        yield return null;

        gm.gameData.isTutorialShowed[(int)type] = true;
        rtArrow.SetActive(false);
        objTextBox.SetActive(false);
        isClicked = false;
        objClick.SetActive(false);
        co_CurrentTutorial = null;
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
                //Debug.Log($"{rtArrow.localPosition} : {time}");
            }

            while(time > 0f)
            {
                rtArrow.localPosition = Vector2.Lerp(originPos, endPos, time);
                time -= Time.deltaTime * 2f;
                yield return null;
                //Debug.Log($"{rtArrow.localPosition} : {time}");
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

    IEnumerator PlayDialog(TutorialType type)
    {
        var tutorialDialogList = DataDialog.GetListByType(type);
        if (tutorialDialogList == null)
            yield break;

        objTextBox.SetActive(true);

        for (int i = 0; i < tutorialDialogList.Count; i++)
        {
            isClicked = false;
            var dataDialog = tutorialDialogList[i];
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
