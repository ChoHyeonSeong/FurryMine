using System;
using System.Collections.Generic;

public enum EEnforce
{
    INVALID = -1,
    HEAD_MINING_POWER = 0,
    HEAD_MINING_SPEED,
    HEAD_MOVING_SPEED,
    HEAD_CRITICAL_PERCENT,
    HEAD_CRITICAL_POWER,
    STAFF_MINER_COUNT,
    COUNT
}

public static class EnforceManager
{
    private static List<int> _levelList = new List<int>();
    private static List<int> _priceList = new List<int>();

    private static Dictionary<EEnforce, int> _enumToId = new Dictionary<EEnforce, int>();

    public static int EnforceCount { get; private set; } = (int)EEnforce.COUNT;

    public static void LoadEnforce()
    {
        InitEnumToId();
        InitLevel();
        InitPrice();
    }

    public static void LevelUpEnforce(EEnforce enforce)
    {
        int id = _enumToId[enforce];
        _levelList[id]++;
        UpdatePrice(id);
    }

    public static int GetPrice(EEnforce enforce)
    {
        return _priceList[_enumToId[enforce]];
    }

    public static int GetLevel(EEnforce enforce)
    {
        return _levelList[_enumToId[enforce]];
    }

    public static float GetCoeff(EEnforce enforce)
    {
        return TableManager.EnforceTable[_enumToId[enforce]].Coeff;
    }

    public static float GetLimit(EEnforce enforce)
    {
        return TableManager.EnforceTable[_enumToId[enforce]].Limit;
    }

    private static void InitEnumToId()
    {
        for (int i = 0; i < EnforceCount; i++)
        {
            int enforceId = TableManager.EnforceTable[i].Id;
            _enumToId[(EEnforce)enforceId] = enforceId;
        }
    }

    private static void InitLevel()
    {
        bool isSaveNull = SaveManager.Save.EnforceLevels == null;
        for (int i = 0; i < EnforceCount; i++)
        {
            _levelList.Add(isSaveNull ? 0 : SaveManager.Save.EnforceLevels[i]);
        }
    }

    private static void InitPrice()
    {
        for (int i = 0; i < _levelList.Count; i++)
        {
            _priceList.Add((_levelList[i] + 1) * TableManager.EnforceTable[i].Price);
        }
    }


    private static void UpdatePrice(int id)
    {
        _priceList[id] = (_levelList[id] + 1) * TableManager.EnforceTable[id].Price;
    }
}
