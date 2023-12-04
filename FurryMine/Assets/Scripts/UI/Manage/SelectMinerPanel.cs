using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMinerPanel : MonoBehaviour
{
    [SerializeField]
    private Button _cancelBtn;
    private Action<int> _callback;
    private SelectMinerContent _selectMinerContent;

    private void Awake()
    {
        _cancelBtn.onClick.AddListener(HideSelectMiner);
        _selectMinerContent =GetComponentInChildren<SelectMinerContent>();
        MinerTeam.OnSetStaffMiner += ShowSelectStaffMiner;
        SelectMinerItem.OnSelectClick += ExecuteCallback;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MinerTeam.OnSetStaffMiner -= ShowSelectStaffMiner;
        SelectMinerItem.OnSelectClick -= ExecuteCallback;
    }

    private void ShowSelectStaffMiner(Action<int> callback)
    {
        _selectMinerContent.InitContent(false);
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
