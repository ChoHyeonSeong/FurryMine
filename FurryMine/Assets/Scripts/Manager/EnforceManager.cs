using System;
using System.Collections.Generic;
using UnityEngine;

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
    public Dictionary<EEnforce, int> LevelDict { get => _levelDict; }

    private Dictionary<EEnforce, int> _levelDict = new Dictionary<EEnforce, int>();
    private Dictionary<EEnforce, int> _priceDict = new Dictionary<EEnforce, int>();
    private Dictionary<EEnforce, float> _coeffDict = new Dictionary<EEnforce, float>();

    private MineCart _cart;

    public void Init()
    {
        var enumArray = Enum.GetValues(typeof(EEnforce));
        InitLevel(enumArray);
        InitPrice();
        InitCoeff();
        InitStat(enumArray);
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

    private void InitLevel(Array enumArray)
    {
        bool isSaveNull = SaveManager.Save.EnforceLevels == null;
        foreach (EEnforce enforce in enumArray)
        {
            _levelDict[enforce] = isSaveNull ? 0 : SaveManager.Save.EnforceLevels[(int)enforce];
        }
    }

    private void InitPrice()
    {
        _priceDict[EEnforce.HEAD_MINING_POWER] = DataManager.PriceDict[_levelDict[EEnforce.HEAD_MINING_POWER]].HeadMiningPower;
        _priceDict[EEnforce.HEAD_MINING_SPEED] = DataManager.PriceDict[_levelDict[EEnforce.HEAD_MINING_SPEED]].HeadMiningSpeed;
        _priceDict[EEnforce.HEAD_MOTION_SPEED] = DataManager.PriceDict[_levelDict[EEnforce.HEAD_MOTION_SPEED]].HeadMotionSpeed;
        _priceDict[EEnforce.HEAD_MOVING_SPEED] = DataManager.PriceDict[_levelDict[EEnforce.HEAD_MOVING_SPEED]].HeadMovingSpeed;
        _priceDict[EEnforce.HEAD_MINING_COUNT] = DataManager.PriceDict[_levelDict[EEnforce.HEAD_MINING_COUNT]].HeadMiningCount;
        _priceDict[EEnforce.HEAD_CRITICAL_PERCENT] = DataManager.PriceDict[_levelDict[EEnforce.HEAD_CRITICAL_PERCENT]].HeadCriticalPercent;
        _priceDict[EEnforce.HEAD_CRITICAL_POWER] = DataManager.PriceDict[_levelDict[EEnforce.HEAD_CRITICAL_POWER]].HeadCriticalPower;
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

    private void InitStat(Array enumArray)
    {
        foreach (EEnforce enforce in enumArray)
        {
            ApplyEnforce(enforce);
        }
    }

    private void ApplyEnforce(EEnforce enforce)
    {
        switch (enforce)
        {
            case EEnforce.HEAD_MINING_POWER:
            case EEnforce.HEAD_MINING_SPEED:
            case EEnforce.HEAD_MOTION_SPEED:
            case EEnforce.HEAD_MOVING_SPEED:
            case EEnforce.HEAD_MINING_COUNT:
            case EEnforce.HEAD_CRITICAL_PERCENT:
            case EEnforce.HEAD_CRITICAL_POWER:
                GameManager.Inst.Player.EnforceStat(enforce, _levelDict[enforce] * _coeffDict[enforce]);
                break;
        }
    }

    private void UpdatePrice(int level, EEnforce enforce)
    {
        int price = 0;
        switch (enforce)
        {
            case EEnforce.HEAD_MINING_POWER:
                price = DataManager.PriceDict[level].HeadMiningPower;
                break;
            case EEnforce.HEAD_MINING_SPEED:
                price = DataManager.PriceDict[level].HeadMiningSpeed;
                break;
            case EEnforce.HEAD_MOTION_SPEED:
                price = DataManager.PriceDict[level].HeadMotionSpeed;
                break;
            case EEnforce.HEAD_MOVING_SPEED:
                price = DataManager.PriceDict[level].HeadMovingSpeed;
                break;
            case EEnforce.HEAD_MINING_COUNT:
                price = DataManager.PriceDict[level].HeadMiningCount;
                break;
            case EEnforce.HEAD_CRITICAL_PERCENT:
                price = DataManager.PriceDict[level].HeadCriticalPercent;
                break;
            case EEnforce.HEAD_CRITICAL_POWER:
                price = DataManager.PriceDict[level].HeadCriticalPower;
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

            ApplyEnforce(enforce);
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
