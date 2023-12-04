using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public SaveData(int money, int ownerLevel, int remainCoolTime, List<int> enforceLevels)
    {
        Money = money;
        OwnerLevel = ownerLevel;
        RemainCoolTime = remainCoolTime;
        EnforceLevels = enforceLevels;
        CurrentHeadId = 0;
        CurrentMineIndex = 0;
        CurrentStaffIds = new List<int>();
        MinerIds = new List<int>
        {
            0,1,2
        };

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
    public int OwnerLevel;
    public int RemainCoolTime;
    public int CurrentHeadId;
    public List<int> CurrentStaffIds;
    public int CurrentMineIndex;
    public List<int> EnforceLevels;
    public List<int> MinerIds;
    public List<MineData> MineDatas;
}
