using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public SaveData()
    {
#if UNITY_EDITOR
        Money = 10000000;
#else
        Money = 0;
#endif
        OwnerLevel = 1;
        AdDateTime = string.Empty;
        CurrentMineIndex = 0;
        CurrentHeadId = 0;
        CurrentStaffIds = new List<int>();
        CurrentMinerEquip = new List<MinerEquip>();
        EnforceLevels = new List<int>();
        for (int i = 0; i < EnforceManager.EnforceCount; i++)
            EnforceLevels.Add(0);
        MineDatas = new List<MineData>
        {
            new MineData
            {
                OreTypeId = 0,
                OreGradeId = 0,
                MineLevelId = 0,
                OreDeposit = -1,
            },
#if UNITY_EDITOR
            new MineData
            {
                OreTypeId = 0,
                OreGradeId = 0,
                MineLevelId = 0,
                OreDeposit = 10,
            },
#endif
        };
        MinerIds = new List<int>
        {
            0,1,2
        };
        EquipIds = new List<int>
        {
            0,1,2
        };
    }

    public int Money;
    public int OwnerLevel;
    public string AdDateTime;
    public int CurrentMineIndex;
    public int CurrentHeadId;
    public List<int> CurrentStaffIds;
    public List<MinerEquip> CurrentMinerEquip;
    public List<int> EnforceLevels;
    public List<MineData> MineDatas;
    public List<int> MinerIds;
    public List<int> EquipIds;
}

[Serializable]
public class MinerEquip
{
    public MinerEquip() : this(0, 0)
    {

    }

    public MinerEquip(int minerId, int equipId)
    {
        MinerId = minerId;
        EquipId = equipId;
    }

    public int MinerId;
    public int EquipId;
}
