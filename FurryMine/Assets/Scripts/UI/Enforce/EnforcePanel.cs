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
        RewardReceiver.OnUpdateEnforce += UpdateText;
    }

    private void OnDestroy()
    {
        RewardReceiver.OnUpdateEnforce -= UpdateText;
    }

    private void UpdateText(EEnforce enforce)
    {
        _items[enforce].SetText(EnforceManager.GetLevel(enforce), EnforceManager.GetCoeff(enforce), EnforceManager.GetPrice(enforce));
    }
}
