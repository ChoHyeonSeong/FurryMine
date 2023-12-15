using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectMinerPanel : MonoBehaviour
{
    public static Action<int, int> OnSetStaffMiner { get; set; }
    public static Action<int, int> OnSetMinerEquip { get; set; }

    [SerializeField]
    private Button _cancelBtn;

    [SerializeField]
    private TextMeshProUGUI _titleText;

    private const string _changeMinerTitle = "±³Ã¼ÇÒ ±¤ºÎ ¼±ÅÃ";
    private const string _minerEquipTitle = "Âø¿ëÇÒ ±¤ºÎ ¼±ÅÃ";
    private bool _isStaff;
    private int _itemId;
    private SelectMinerContent _selectMinerContent;

    private void Awake()
    {
        _cancelBtn.onClick.AddListener(HideSelectMiner);
        _selectMinerContent = GetComponentInChildren<SelectMinerContent>();
        MinerTeam.OnSelectMiner += ShowSelectStaffMiner;
        EquipItem.OnClickWear += ShowSelectMinerEquip;
        SelectMinerItem.OnClickSelect += SelectMiner;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MinerTeam.OnSelectMiner -= ShowSelectStaffMiner;
        EquipItem.OnClickWear -= ShowSelectMinerEquip;
        SelectMinerItem.OnClickSelect -= SelectMiner;
    }

    private void ShowSelectStaffMiner(int minerId)
    {
        _isStaff = true;
        _titleText.text = _changeMinerTitle;
        _selectMinerContent.InitContent(false);
        _itemId = minerId;
        gameObject.SetActive(true);
    }

    private void ShowSelectMinerEquip(int equipId)
    {
        _isStaff = false;
        _titleText.text = _minerEquipTitle;
        _selectMinerContent.InitContent(true);
        _itemId = equipId;
        gameObject.SetActive(true);
    }

    private void SelectMiner(int minerId)
    {
        if (_isStaff)
            OnSetStaffMiner(_itemId, minerId);
        else
            OnSetMinerEquip(_itemId, minerId);
        HideSelectMiner();
    }

    private void HideSelectMiner()
    {
        _selectMinerContent.PoolContent();
        _itemId = 0;
        gameObject.SetActive(false);
    }

}
