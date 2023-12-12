using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public SaveData()
    {
        Money = 0;
        OwnerLevel = 1;
        AdDateTime = null;
        CurrentMineIndex = 0;
        CurrentHeadId = 0;
        CurrentStaffIds = new List<int>();
        CurrentMinerEquip = new Dictionary<int, int>();
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
            }
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
    public Dictionary<int, int> CurrentMinerEquip;
    public List<int> EnforceLevels;
    public List<MineData> MineDatas;
    public List<int> MinerIds;
    public List<int> EquipIds;
}
