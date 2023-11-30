using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TableManager
{
    public static Dictionary<int, MineEntity> MineTable = new Dictionary<int, MineEntity>();
    public static Dictionary<int, EnforceEntity> EnforceTable = new Dictionary<int, EnforceEntity>();

    public static void LoadTable()
    {
        var mineData = Resources.Load<MineTable>("Datas/MineTable");
        foreach (var entity in mineData.Table)
        {
            MineTable[entity.Level] = entity;
        }

        var enforceData = Resources.Load<EnforceTable>("Datas/EnforceTable");
        foreach (var entity in enforceData.Table)
        {
            EnforceTable[entity.Id] = entity;
        }
    }
}
