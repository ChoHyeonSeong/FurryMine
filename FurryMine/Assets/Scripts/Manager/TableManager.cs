using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TableManager
{
    public static Dictionary<int, MineEntity> MineTable = new Dictionary<int, MineEntity>();
    public static Dictionary<int, PriceEntity> PriceTable = new Dictionary<int, PriceEntity>();

    public static void LoadTable()
    {
        var mineData = Resources.Load<MineTable>("Datas/MineTable");
        foreach (var entity in mineData.Table)
        {
            MineTable[entity.Level] = entity;
        }

        var enforcePriceData = Resources.Load<PriceTable>("Datas/PriceTable");
        foreach (var entity in enforcePriceData.Table)
        {
            PriceTable[entity.Level] = entity;
        }
    }
}
