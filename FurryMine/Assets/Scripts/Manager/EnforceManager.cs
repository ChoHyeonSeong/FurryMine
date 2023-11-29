using System;
using System.Collections.Generic;

public enum EEnforce
{
    HEAD_MINING_POWER,
    HEAD_MINING_SPEED,
    HEAD_MOTION_SPEED,
    HEAD_MOVING_SPEED,
    //HEAD_MINING_COUNT,
    HEAD_CRITICAL_PERCENT,
    HEAD_CRITICAL_POWER,
}

public static class EnforceManager
{
    public static Dictionary<EEnforce, int> LevelDict { get; private set; } = new Dictionary<EEnforce, int>();
    public static Dictionary<EEnforce, int> PriceDict { get; private set; } = new Dictionary<EEnforce, int>();
    public static Dictionary<EEnforce, float> CoeffDict { get; private set; } = new Dictionary<EEnforce, float>();
    public static Dictionary<EEnforce, int> LimitDict { get; private set; } = new Dictionary<EEnforce, int>();

    public static void LoadEnforce()
    {
        InitLevel();
        InitPrice();
        InitCoeff();
        InitLimit();
    }

    private static void InitLimit()
    {
        LimitDict[EEnforce.HEAD_MINING_POWER] = 1000;
        LimitDict[EEnforce.HEAD_MINING_SPEED] = 1000;
        LimitDict[EEnforce.HEAD_MOTION_SPEED] = 1000;
        LimitDict[EEnforce.HEAD_MOVING_SPEED] = 1000;
        LimitDict[EEnforce.HEAD_CRITICAL_PERCENT] = 100;
        LimitDict[EEnforce.HEAD_CRITICAL_POWER] = 1000;
    }

    public static void LevelUpEnforce(EEnforce enforce)
    {
        LevelDict[enforce]++;
        UpdatePrice(enforce);
    }

    public static List<int> GetEnforceLevelList()
    {
        List<int> list = new List<int>();
        var enumArray = Enum.GetValues(typeof(EEnforce));
        foreach (EEnforce enforce in enumArray)
        {
            list.Add(LevelDict[enforce]);
        }
        return list;
    }

    private static void InitLevel()
    {
        var enumArray = Enum.GetValues(typeof(EEnforce));
        bool isSaveNull = SaveManager.Save.EnforceLevels == null;
        foreach (EEnforce enforce in enumArray)
        {
            LevelDict[enforce] = isSaveNull ? 0 : SaveManager.Save.EnforceLevels[(int)enforce];
        }
    }

    private static void InitPrice()
    {
        PriceDict[EEnforce.HEAD_MINING_POWER] = TableManager.PriceTable[LevelDict[EEnforce.HEAD_MINING_POWER]].HeadMiningPower;
        PriceDict[EEnforce.HEAD_MINING_SPEED] = TableManager.PriceTable[LevelDict[EEnforce.HEAD_MINING_SPEED]].HeadMiningSpeed;
        PriceDict[EEnforce.HEAD_MOTION_SPEED] = TableManager.PriceTable[LevelDict[EEnforce.HEAD_MOTION_SPEED]].HeadMotionSpeed;
        PriceDict[EEnforce.HEAD_MOVING_SPEED] = TableManager.PriceTable[LevelDict[EEnforce.HEAD_MOVING_SPEED]].HeadMovingSpeed;
        PriceDict[EEnforce.HEAD_CRITICAL_PERCENT] = TableManager.PriceTable[LevelDict[EEnforce.HEAD_CRITICAL_PERCENT]].HeadCriticalPercent;
        PriceDict[EEnforce.HEAD_CRITICAL_POWER] = TableManager.PriceTable[LevelDict[EEnforce.HEAD_CRITICAL_POWER]].HeadCriticalPower;
    }

    private static void InitCoeff()
    {
        CoeffDict[EEnforce.HEAD_MINING_POWER] = 1f;
        CoeffDict[EEnforce.HEAD_MINING_SPEED] = 0.01f;
        CoeffDict[EEnforce.HEAD_MOTION_SPEED] = 0.01f;
        CoeffDict[EEnforce.HEAD_MOVING_SPEED] = 0.01f;
        CoeffDict[EEnforce.HEAD_CRITICAL_PERCENT] = 0.01f;
        CoeffDict[EEnforce.HEAD_CRITICAL_POWER] = 0.01f;
    }

    private static void UpdatePrice(EEnforce enforce)
    {
        int price;
        switch (enforce)
        {
            case EEnforce.HEAD_MINING_POWER:
                price = TableManager.PriceTable[LevelDict[enforce]].HeadMiningPower;
                break;
            case EEnforce.HEAD_MINING_SPEED:
                price = TableManager.PriceTable[LevelDict[enforce]].HeadMiningSpeed;
                break;
            case EEnforce.HEAD_MOTION_SPEED:
                price = TableManager.PriceTable[LevelDict[enforce]].HeadMotionSpeed;
                break;
            case EEnforce.HEAD_MOVING_SPEED:
                price = TableManager.PriceTable[LevelDict[enforce]].HeadMovingSpeed;
                break;
            case EEnforce.HEAD_CRITICAL_PERCENT:
                price = TableManager.PriceTable[LevelDict[enforce]].HeadCriticalPercent;
                break;
            case EEnforce.HEAD_CRITICAL_POWER:
                price = TableManager.PriceTable[LevelDict[enforce]].HeadCriticalPower;
                break;
            default:
                price = 0;
                break;
        }
        PriceDict[enforce] = price;
    }
}
