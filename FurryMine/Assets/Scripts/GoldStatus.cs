using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldStatus : MonoBehaviour
{
    private TextMeshProUGUI _moneyText;
    private void Awake()
    {
        _moneyText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        MineCart.OnPlusMoney += UpdateMoney;
    }

    private void OnDisable()
    {
        MineCart.OnPlusMoney -= UpdateMoney;
    }

    private void UpdateMoney(int money)
    {
        _moneyText.text = string.Format("{0:N0}", money);
    }
}
