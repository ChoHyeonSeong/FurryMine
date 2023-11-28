using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static Dictionary<int, MineEntity> MineDict = new Dictionary<int, MineEntity>();
    public static Dictionary<int, PriceEntity> PriceDict = new Dictionary<int, PriceEntity>();

    public static void LoadData()
    {
        var mineData = Resources.Load<MineTable>("Datas/MineTable");
        foreach (var entity in mineData.Table)
        {
            MineDict[entity.Level] = entity;
        }

        var enforcePriceData = Resources.Load<PriceTable>("Datas/PriceTable");
        foreach (var entity in enforcePriceData.Table)
        {
            PriceDict[entity.Level] = entity;
        }
    }
}
