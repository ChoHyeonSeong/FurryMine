using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public SaveData(int money, int mineLevel, int remainCoolTime, List<int> enforceLevels)
    {
        Money = money;
        MineLevel = mineLevel;
        RemainCoolTime = remainCoolTime;
        HeadMinerId = 0;
        EnforceLevels = enforceLevels;
        CurrentMineIndex = 0;
        MineDatas = new List<MineData>
        {
            new MineData
            {
                OreId = 0,
                OreDeposit = -1,
                OreHealth = 2,
                RespawnTime = 1,
                OreCount = 2,
                MineralCount = 1,
                MineralPrice = 1,
            }
        };
    }

    public SaveData() : this(0, 1, 0, null) { }

    public int Money;
    public int MineLevel;
    public int RemainCoolTime;
    public int HeadMinerId;
    public int CurrentMineIndex;
    public List<int> EnforceLevels;
    public List<MineData> MineDatas;
}
