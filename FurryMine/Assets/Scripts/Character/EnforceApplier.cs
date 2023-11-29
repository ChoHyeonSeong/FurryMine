using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceApplier : MonoBehaviour
{
    private HeadMiner _headMiner;
    private MineCart _mineCart;

    private void OnEnable()
    {
        GameApp.OnGameStart += GameStart;
        EnforceItem.OnBuyEnforce += BuyEnforceItem;
    }

    private void OnDisable()
    {
        GameApp.OnGameStart -= GameStart;
        EnforceItem.OnBuyEnforce -= BuyEnforceItem;
    }

    private void GameStart()
    {
        _headMiner = GameManager.Player;
        _mineCart = GameManager.Cart;
        var enumArray = Enum.GetValues(typeof(EEnforce));
        foreach (EEnforce enforce in enumArray)
        {
            ApplyEnforce(enforce);
        }
    }

    private void BuyEnforceItem(EnforceItem item)
    {
        EEnforce enforce = item.Enforce;
        if (_mineCart.MinusMoney(EnforceManager.PriceDict[enforce]))
        {
            EnforceManager.LevelUpEnforce(enforce);
            ApplyEnforce(enforce);
            item.SetText(EnforceManager.LevelDict[enforce], EnforceManager.CoeffDict[enforce], EnforceManager.PriceDict[enforce]);
        }
    }

    private void ApplyEnforce(EEnforce enforce)
    {
        _headMiner.EnforceStat(enforce, EnforceManager.LevelDict[enforce] * EnforceManager.CoeffDict[enforce]);
    }
}
