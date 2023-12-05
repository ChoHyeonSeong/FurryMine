using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectMinerPanel : MonoBehaviour
{
    [SerializeField]
    private Button _cancelBtn;

    [SerializeField]
    private TextMeshProUGUI _titleText;

    private const string _changeMinerTitle = "±³Ã¼ÇÒ ±¤ºÎ ¼±ÅÃ";
    private const string _minerEquipTitle = "Âø¿ëÇÒ ±¤ºÎ ¼±ÅÃ";
    private Action<int> _callback;
    private SelectMinerContent _selectMinerContent;

    private void Awake()
    {
        _cancelBtn.onClick.AddListener(HideSelectMiner);
        _selectMinerContent =GetComponentInChildren<SelectMinerContent>();
        MinerTeam.OnSetStaffMiner += ShowSelectStaffMiner;
        MinerTeam.OnSetMinerEquip += ShowSelectMinerEquip;
        SelectMinerItem.OnSelectClick += ExecuteCallback;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MinerTeam.OnSetStaffMiner -= ShowSelectStaffMiner;
        MinerTeam.OnSetMinerEquip -= ShowSelectMinerEquip;
        SelectMinerItem.OnSelectClick -= ExecuteCallback;
    }

    private void ShowSelectStaffMiner(Action<int> callback)
    {
        _titleText.text = _changeMinerTitle;
        _selectMinerContent.InitContent(false);
        _callback = callback;
        gameObject.SetActive(true);
    }

    private void ShowSelectMinerEquip(Action<int> callback)
    {
        _titleText.text = _minerEquipTitle;
        _selectMinerContent.InitContent(true);
        _callback = callback;
        gameObject.SetActive(true);
    }
    
    private void ExecuteCallback(int minerId)
    {
        _callback(minerId);
        HideSelectMiner();
    }

    private void HideSelectMiner()
    {
        _selectMinerContent.PoolContent();
        _callback = null;
        gameObject.SetActive(false);
    }

}
