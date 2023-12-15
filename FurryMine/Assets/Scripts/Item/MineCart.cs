using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCart : MonoBehaviour
{
    public static Action<bool, string, Vector2> OnPlusText { get; set; }
    public static Action<int> OnChangeMoney { get; set; }

    public int Money { get => _money; }

    private int _money;

    public void GameStart()
    {
        _money = SaveManager.Save.Money;
        OnChangeMoney(_money);
    }

    public void InsertMoney(int price)
    {
        _money += price;
        OnChangeMoney(_money);
    }

    public void PlusMoney(int price)
    {
        _money += price;
        OnChangeMoney(_money);
        OnPlusText(true, $"+{price}G", transform.position);
    }

    public bool MinusMoney(int price)
    {
        if (price <= _money)
        {
            _money -= price;
            OnChangeMoney(_money);
            return true;
        }
        return false;
    }


    private void OnEnable()
    {
        GameApp.OnGameStart += GameStart;
        Mine.OnSellMine += InsertMoney;
        AddGoldButton.OnAddGold += InsertMoney;
    }

    private void OnDisable()
    {
        GameApp.OnGameStart -= GameStart;
        Mine.OnSellMine -= InsertMoney;
        AddGoldButton.OnAddGold -= InsertMoney;
    }
}
