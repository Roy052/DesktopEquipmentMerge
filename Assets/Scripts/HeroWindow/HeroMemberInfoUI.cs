using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroMemberInfoUI : WindowUI
{
    const float GaugeMax = 130f;

    public Text textName;
    public Text textLv;
    public RectTransform rtGauge;
    public Text textPower;

    [Header("ExpItem")]
    public GameObject objExpItemBox;
    public EltItem eltExpItem;
    List<EltItem> eltExpItems = new();
    List<int> expItemIds = new();

    public EltItem eltWeapon;
    public EltItem eltArmor;

    public HeroMemberEquipmentUI heroMemberEquipmentUI;

    InfoHero infoHero;
    DataHero dataHero;

    public void Awake()
    {
        Observer.onRefreshHeroes += RefreshHero;
    }

    public void OnDestroy()
    {
        Observer.onRefreshHeroes -= RefreshHero;
    }

    public override void Hide()
    {
        base.Hide();
        heroMemberEquipmentUI.Hide();
    }

    public void Set(InfoHero infoHero)
    {
        this.infoHero = infoHero;
        dataHero = DataHero.Get(infoHero.heroId);
        textName.text = infoHero.strName;
        short lv = DataLv.GetLv(infoHero.exp);
        textLv.text = $"Lv.{lv}";

        var dataLv = DataLv.Get(lv);
        int bottomValue = dataLv.expMax - dataLv.expMin;
        int upValue = infoHero.exp - dataLv.expMin;
        rtGauge.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GaugeMax * Mathf.Min(1, (upValue/ (float) bottomValue)));

        int powerValue = 0;
        powerValue = lv * 10;
        powerValue += (DataMergeItem.Get(infoHero.weaponId)?.grade ?? 0) * 100;
        powerValue += (DataMergeItem.Get(infoHero.armorId)?.grade ?? 0) * 100;
        textPower.text = powerValue.ToString();

        eltWeapon.Set(infoHero.weaponId, -1);
        eltWeapon.funcClick = OnClickWeapon;
        eltArmor.Set(infoHero.armorId, -1);
        eltArmor.funcClick = OnClickArmor;
        heroMemberEquipmentUI.Hide();
    }

    public void OnClickShowExpItem()
    {
        objExpItemBox.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            int itemId = DataMergeItem.GetByTypeGrade(MergeItemType.ExpBook, (short)(i + 1)).id;
            if (expItemIds.Count <= i)
                expItemIds.Add(itemId);

            var elt = Utilities.GetOrCreate(eltExpItems, i, eltExpItem.gameObject);
            elt.Set(itemId, i);
            elt.funcClick = OnClickUseExpItem;
            elt.SetActive(true);

            bool isItemExist = Singleton.gm.gameData.itemCounts.TryGetValue(itemId, out var count) == false || count == 0;
            elt.GetComponent<ButtonAddOn>().ChangeGrayScale(isItemExist);
        }

        Utilities.DeactivateSurplus(eltExpItems, 3);
    }

    public void OnClickUseExpItem(int idx)
    {
        int itemId = expItemIds[idx];
        if (Singleton.gm.gameData.itemCounts.TryGetValue(itemId, out var count) == false || count == 0)
            return;

        DataMergeItem dataMergeItem = DataMergeItem.Get(itemId);
        if(dataMergeItem == null)
        {
            Debug.LogError($"Data MergeItem Not Exist {itemId}");
            return;
        }
        infoHero.exp += dataMergeItem.price;
        Singleton.gm.gameData.itemCounts[itemId]--;
        Observer.onRefreshHeroes?.Invoke();
        Observer.onRefreshItems?.Invoke();
    }

    public void OnClickWeapon(int dummy)
    {
        if (dataHero == null)
            return;

        heroMemberEquipmentUI.Show();
        heroMemberEquipmentUI.Set(dataHero.weapon);
        heroMemberEquipmentUI.funcClick = OnClickWeaponItem;
    }

    public void OnClickArmor(int dummy)
    {
        if (dataHero == null)
            return;

        heroMemberEquipmentUI.Show();
        heroMemberEquipmentUI.Set(dataHero.armor);
        heroMemberEquipmentUI.funcClick = OnClickArmorItem;
    }

    public void OnClickWeaponItem(int itemId)
    {
        Singleton.gm.gameData.ChangeEquipment(infoHero, true, itemId);
    }

    public void OnClickArmorItem(int itemId)
    {
        Singleton.gm.gameData.ChangeEquipment(infoHero, false, itemId);
    }

    public void RefreshHero()
    {
        Set(infoHero);
    }
}
