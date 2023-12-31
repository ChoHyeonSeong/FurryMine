using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceApplier : MonoBehaviour
{
    private MineCart _mineCart;
    private MinerTeam _minerTeam;
    private Mine _mine;
    private Snack _snack;

    private void Awake()
    {
        _mine = GetComponentInChildren<Mine>();
        _minerTeam = GetComponentInChildren<MinerTeam>();
        _snack = GetComponentInChildren<Snack>();
    }

    private void OnEnable()
    {
        GameApp.OnGameStart += GameStart;
        EnforceItem.OnBuyEnforce += BuyEnforceItem;
        RewardReceiver.OnRandomEnforce += ApplyEnforce;
    }

    private void OnDisable()
    {
        GameApp.OnGameStart -= GameStart;
        EnforceItem.OnBuyEnforce -= BuyEnforceItem;
        RewardReceiver.OnRandomEnforce -= ApplyEnforce;
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
            bool isMaxEnforce = EnforceManager.GetLevel(enforce) >= EnforceManager.GetLimit(enforce);
            item.SetText(
                EnforceManager.GetLevel(enforce),
                EnforceManager.GetCoeff(enforce),
                isMaxEnforce ? 0 : EnforceManager.GetPrice(enforce)
                );
            if (isMaxEnforce)
                item.BlockEnforce();
        }
    }

    private void ApplyEnforce(EEnforce enforce)
    {
        switch (enforce)
        {
            case EEnforce.HEAD_MINING_POWER:
            case EEnforce.HEAD_MINING_SPEED:
            case EEnforce.HEAD_MOVING_SPEED:
            case EEnforce.HEAD_CRITICAL_PERCENT:
            case EEnforce.HEAD_CRITICAL_POWER:
                _minerTeam.EnforceHead(enforce);
                break;
            case EEnforce.STAFF_MINER_COUNT:
            case EEnforce.STAFF_MINING_POWER:
            case EEnforce.STAFF_MINING_SPEED:
            case EEnforce.STAFF_MOVING_SPEED:
                _minerTeam.EnforceStaff(enforce);
                break;
            case EEnforce.MINE_RESPAWN_SPEED:
            case EEnforce.MINE_ORE_COUNT:
            case EEnforce.MINE_MINERAL_COUNT:
            case EEnforce.MINE_MINERAL_PRICE:
                _mine.EnforceMine(enforce);
                break;
            case EEnforce.SNACK_CHARGE_SPEED:
            case EEnforce.SNACK_DURATION_RATIO:
            case EEnforce.SNACK_MINING_POWER_BUFF:
            case EEnforce.SNACK_MINING_SPEED_BUFF:
            case EEnforce.SNACK_MOVING_SPEED_BUFF:
                _snack.EnforceSnack(enforce);
                break;
            default:
                Debug.Log("지정되지 않은 강화");
                break;
        }
    }
}
