using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public SaveData(int money, int mineLevel, List<int> enforceLevels)
    {
        Money = money;
        MineLevel = mineLevel;
        EnforceLevels = enforceLevels;
    }

    public SaveData() : this(0, 1, null) { }

    public int Money;
    public int MineLevel;
    public List<int> EnforceLevels;
}
