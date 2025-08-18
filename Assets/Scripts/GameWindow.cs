using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameWindow : Singleton, IBeginDragHandler, IDragHandler
{
    public Vector2 PosOrigin;

    [Header("LvUpUI")]
    public GameObject objAlarm;
    public BuildingType type = BuildingType.None;
    public BuildingLvUpUI buildingLvUpUI;

    public bool IsShow => gameObject.activeSelf;

    RectTransform rt;
    Canvas canvas;
    Vector2 offset;

    protected virtual void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        Observer.onRefreshItems += RefreshLvUpBtn;
    }

    protected virtual void OnDestroy()
    {
        Observer.onRefreshItems -= RefreshLvUpBtn;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            e.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos);

        offset = localPos - rt.anchoredPosition;
    }

    // 드래그 중 계속 호출
    public void OnDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            e.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos);

        rt.anchoredPosition = localPos - offset;
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        if (rt.localPosition.x > 960f || rt.localPosition.x < -960f || rt.localPosition.y > 540f || rt.localPosition.y < -540f)
            rt.localPosition = PosOrigin;
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }


    public void RefreshLvUpBtn()
    {
        short lv = gm.gameData.buildingLvs[(int)type];
        DataBuilding dataBuilding = DataBuilding.Get(type, lv);
        if (dataBuilding == null)
        {
            Debug.LogError($"Data Building Not Exist {type} {lv}");
            return;
        }

        bool isConditionClear = gm.gameData.IsConditionClear(dataBuilding.conditions);
        objAlarm.SetActive(isConditionClear);
    }

    public void OnClickBuildingLvUp()
    {
        if (type == BuildingType.None)
            return;

        if (buildingLvUpUI == null)
            return;

        buildingLvUpUI.SetActive(true);
        buildingLvUpUI.Set(type, gm.gameData.buildingLvs[(int)type]);
    }
}
