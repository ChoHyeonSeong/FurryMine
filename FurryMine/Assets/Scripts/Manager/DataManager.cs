using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static Dictionary<int, MineEntity> MineDict = new Dictionary<int, MineEntity>();
    public static Dictionary<int, EnforcePriceEntity> EnforcePriceDict = new Dictionary<int, EnforcePriceEntity>();

    public static void LoadData()
    {
        var mineData = Resources.Load<MineData>("Datas/MineData");
        foreach (var entity in mineData.Data)
        {
            MineDict[entity.Level] = entity;
        }

        var enforcePriceData = Resources.Load<EnforcePriceData>("Datas/EnforcePriceData");
        foreach (var entity in enforcePriceData.Data)
        {
            EnforcePriceDict[entity.Level] = entity;
        }
    }
}
