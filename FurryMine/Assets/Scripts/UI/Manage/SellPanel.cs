using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SellPanel : MonoBehaviour
{
    public static Action<MineItem> OnClickConfirmSelling { get; set; }

    [SerializeField]
    private TextMeshProUGUI _sellContent;
    [SerializeField]
    private Button _confirmBtn;
    [SerializeField]
    private Button _cancelBtn;

    private MineItem _sellMine;

    private void Awake()
    {
        _confirmBtn.onClick.AddListener(ClickConfirm);
        _cancelBtn.onClick.AddListener(ClickCancel);
        MineItem.OnSellClick += ShowSellPanel;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MineItem.OnSellClick -= ShowSellPanel;
    }

    private void ShowSellPanel(MineItem item)
    {
        MineData data = GameManager.Mine.MineDataList[item.MineIndex];
        OreTypeEntity oreTypeEntity = TableManager.OreTypeTable[data.OreTypeId];
        OreGradeEntity oreGradeEntity = TableManager.OreGradeTable[data.OreGradeId];
        int sellPrice = (int)(data.OreDeposit * oreTypeEntity.MineralPrice * oreGradeEntity.MineralCount * 0.6f);
        int miningPrice = data.OreDeposit *
            (oreTypeEntity.MineralPrice + EnforceManager.GetLevel(EEnforce.MINE_MINERAL_PRICE) * (int)EnforceManager.GetCoeff(EEnforce.MINE_MINERAL_PRICE)) *
            (oreGradeEntity.MineralCount + EnforceManager.GetLevel(EEnforce.MINE_MINERAL_COUNT) * (int)EnforceManager.GetCoeff(EEnforce.MINE_MINERAL_COUNT)
            );
        _sellContent.text = $"�Ű� �� <sprite=0>{sellPrice} �� ��� ������ ������,\n ä���Ϸ� �� ���� <sprite=0>{miningPrice} �� ����ϴ�.\n\n������ �Ű� �Ͻðڽ��ϱ�?";
        _sellMine = item;
        gameObject.SetActive(true);
    }

    private void ClickConfirm()
    {
        OnClickConfirmSelling(_sellMine);
        gameObject.SetActive(false);
    }

    private void ClickCancel()
    {
        gameObject.SetActive(false);
    }
}
