using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceApplier : MonoBehaviour
{
    private Miner _headMiner;
    private MineCart _mineCart;

    private void OnEnable()
    {
        GameApp.OnGameStart += GameStart;
        EnforceItem.OnBuyEnforce += BuyEnforceItem;
        RewardReceiver.OnRandEnforce += ApplyEnforce;
    }

    private void OnDisable()
    {
        GameApp.OnGameStart -= GameStart;
        EnforceItem.OnBuyEnforce -= BuyEnforceItem;
        RewardReceiver.OnRandEnforce -= ApplyEnforce;
    }

    private void GameStart()
    {
        _mineCart = GameManager.Cart;
        for (int enforce = 0; enforce < EnforceManager.EnforceCount; enforce++)
        {
            ApplyEnforce((EEnforce)enforce);
        }
    }

    private void BuyEnforceItem(EnforceItem item)
    {
        EEnforce enforce = item.Enforce;
        if (_mineCart.MinusMoney(EnforceManager.GetPrice(enforce)))
        {
            EnforceManager.LevelUpEnforce(enforce);
            ApplyEnforce(enforce);
            item.SetText(
                EnforceManager.GetLevel(enforce),
                EnforceManager.GetCoeff(enforce),
                EnforceManager.GetLevel(enforce) >= EnforceManager.GetLimit(enforce) ? 0 : EnforceManager.GetPrice(enforce)
                );
        }
    }

    private void ApplyEnforce(EEnforce enforce)
    {
        _headMiner.EnforceStat(enforce, EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
    }
}
