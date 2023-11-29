using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforcePanel : MonoBehaviour
{
    private Dictionary<EEnforce, EnforceItem> _items;

    private void Awake()
    {
        _items = new Dictionary<EEnforce, EnforceItem>();
        var array = GetComponentsInChildren<EnforceItem>();
        foreach (var item in array)
        {
            _items[item.Enforce] = item;
        }
    }

    private void OnEnable()
    {
        RewardReceiver.OnRandEnforce += UpdateItem;
    }

    private void OnDisable()
    {
        RewardReceiver.OnRandEnforce -= UpdateItem;
    }

    private void UpdateItem(EEnforce enforce)
    {
        _items[enforce].SetText(EnforceManager.LevelDict[enforce], EnforceManager.CoeffDict[enforce], EnforceManager.PriceDict[enforce]);
    }
}
