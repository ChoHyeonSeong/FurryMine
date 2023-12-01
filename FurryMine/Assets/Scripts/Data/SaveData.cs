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
    }

    public SaveData() : this(0, 1, 0, null) { }

    public int Money;
    public int MineLevel;
    public int RemainCoolTime;
    public int HeadMinerId;
    public List<int> EnforceLevels;
}
