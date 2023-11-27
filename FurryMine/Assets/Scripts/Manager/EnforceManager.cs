using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public enum EEnforce
{
    HEAD_MINING_POWER,
    HEAD_MINING_SPEED,
    HEAD_MOTION_SPEED,
    HEAD_MOVING_SPEED,
    HEAD_MINING_COUNT,
    HEAD_CRITICAL_PERCENT,
    HEAD_CRITICAL_POWER,
}

public class EnforceManager : MonoBehaviour
{
    private Dictionary<EEnforce, int> _levelDict = new Dictionary<EEnforce, int>();
    private Dictionary<EEnforce, int> _priceDict = new Dictionary<EEnforce, int>();
    private Dictionary<EEnforce, float> _coeffDict = new Dictionary<EEnforce, float>();

    private MineCart _cart;

    public void Init()
    {
        foreach(EEnforce enforce in Enum.GetValues(typeof(EEnforce)))
        {
            _levelDict[enforce] = 0;
        }
        InitPrice();
        InitCoeff();
    }

    private void Start()
    {
        _cart = GameManager.Inst.Cart;
    }

    private void OnEnable()
    {
        EnforceItem.OnBuyEnforce += TryEnforce;
        EnforceItem.OnInitInformation += InitItem;
    }

    private void OnDisable()
    {
        EnforceItem.OnBuyEnforce -= TryEnforce;
        EnforceItem.OnInitInformation -= InitItem;
    }

    private void InitPrice()
    {
        _priceDict[EEnforce.HEAD_MINING_POWER] = DataManager.EnforcePriceDict[1].HeadMiningPower;
        _priceDict[EEnforce.HEAD_MINING_SPEED] = DataManager.EnforcePriceDict[1].HeadMiningSpeed;
        _priceDict[EEnforce.HEAD_MOTION_SPEED] = DataManager.EnforcePriceDict[1].HeadMotionSpeed;
        _priceDict[EEnforce.HEAD_MOVING_SPEED] = DataManager.EnforcePriceDict[1].HeadMovingSpeed;
        _priceDict[EEnforce.HEAD_MINING_COUNT] = DataManager.EnforcePriceDict[1].HeadMiningCount;
        _priceDict[EEnforce.HEAD_CRITICAL_PERCENT] = DataManager.EnforcePriceDict[1].HeadCriticalPercent;
        _priceDict[EEnforce.HEAD_CRITICAL_POWER] = DataManager.EnforcePriceDict[1].HeadCriticalPower;
    }


    private void InitCoeff()
    {
        _coeffDict[EEnforce.HEAD_MINING_POWER] = 1f;
        _coeffDict[EEnforce.HEAD_MINING_SPEED] = 0.01f;
        _coeffDict[EEnforce.HEAD_MOTION_SPEED] = 0.01f;
        _coeffDict[EEnforce.HEAD_MOVING_SPEED] = 0.01f;
        _coeffDict[EEnforce.HEAD_MINING_COUNT] = 1f;
        _coeffDict[EEnforce.HEAD_CRITICAL_PERCENT] = 0.01f;
        _coeffDict[EEnforce.HEAD_CRITICAL_POWER] = 0.01f;
    }

    private void UpdatePrice(int level, EEnforce enforce)
    {
        int price = 0;
        switch (enforce)
        {
            case EEnforce.HEAD_MINING_POWER:
                price = DataManager.EnforcePriceDict[level].HeadMiningPower;
                break;
            case EEnforce.HEAD_MINING_SPEED:
                price = DataManager.EnforcePriceDict[level].HeadMiningSpeed;
                break;
            case EEnforce.HEAD_MOTION_SPEED:
                price = DataManager.EnforcePriceDict[level].HeadMotionSpeed;
                break;
            case EEnforce.HEAD_MOVING_SPEED:
                price = DataManager.EnforcePriceDict[level].HeadMovingSpeed;
                break;
            case EEnforce.HEAD_MINING_COUNT:
                price = DataManager.EnforcePriceDict[level].HeadMiningCount;
                break;
            case EEnforce.HEAD_CRITICAL_PERCENT:
                price = DataManager.EnforcePriceDict[level].HeadCriticalPercent;
                break;
            case EEnforce.HEAD_CRITICAL_POWER:
                price = DataManager.EnforcePriceDict[level].HeadCriticalPower;
                break;
        }
        _priceDict[enforce] = price;
    }


    private void TryEnforce(EnforceItem item)
    {
        EEnforce enforce = item.Enforce;
        if (_cart.MinusMoney(_priceDict[enforce]))
        {
            _levelDict[enforce]++;
            UpdatePrice(_levelDict[enforce], enforce);
            item.SetText(
                _levelDict[enforce],
                (int)(_coeffDict[enforce] * (item.Unit == EUnit.NONE ? 1 : 101)),
                _priceDict[enforce]
                );

            switch (item.Enforce)
            {
                case EEnforce.HEAD_MINING_POWER:
                case EEnforce.HEAD_MINING_SPEED:
                case EEnforce.HEAD_MOTION_SPEED:
                case EEnforce.HEAD_MOVING_SPEED:
                case EEnforce.HEAD_MINING_COUNT:
                case EEnforce.HEAD_CRITICAL_PERCENT:
                case EEnforce.HEAD_CRITICAL_POWER:
                    GameManager.Inst.Player.EnforceStat(item.Enforce, _levelDict[enforce] * _coeffDict[enforce]);
                    break;
            }
        }
    }

    private void InitItem(EnforceItem item)
    {
        EEnforce enforce = item.Enforce;
        item.SetText(
            _levelDict[enforce],
            (int)(_coeffDict[enforce] * (item.Unit == EUnit.NONE ? 1 : 100)),
            _priceDict[enforce]
            );
    }
}
