using System;
using System.Collections;
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
    private Dictionary<EEnforce, int> _levelDict;
    private Dictionary<EEnforce, int> _priceDict;
    private Dictionary<EEnforce, float> _coeffDict;

    private MineCart _cart;

    public void ZeroInit()
    {
        foreach(EEnforce enforce in Enum.GetValues(typeof(EEnforce)))
        {
            _levelDict[enforce] = 0;
            _priceDict[enforce] = 10;
        }
        InitCoeff();
    }

    private void Awake()
    {
        _levelDict = new Dictionary<EEnforce, int>();
        _priceDict = new Dictionary<EEnforce, int>();
        _coeffDict = new Dictionary<EEnforce, float>();

        ZeroInit();
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

    private void TryEnforce(EnforceItem item)
    {
        EEnforce enforce = item.Enforce;
        if (_cart.MinusMoney(_priceDict[enforce]))
        {
            _levelDict[enforce]++;
            _priceDict[enforce] = (int)(_priceDict[enforce] * Consts.GoldenRatio);
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
