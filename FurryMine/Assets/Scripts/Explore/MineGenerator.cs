using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MineGenerator
{
    // 1. 광산 레벨 정하기
    // 2. 광석 종류 정하기
    // 3. 광석 등급 정하기

    public static MineData GenerateMine()
    {
        float mineLevelRand = Random.value * 100;
        float oreTypeRand = Random.value * 100;
        float oreGradeRand = Random.value * 100;

        int mineLevelId = 0;
        int oreTypeId = 0;
        int oreGradeId = 0;

        for (int i = 0; i < TableManager.MineLevelTable.Count; i++)
            if (mineLevelRand >= TableManager.MineLevelTable[i].Probability)
                mineLevelId = i;
        for (int i = 0; i < TableManager.OreTypeTable.Count; i++)
            if (oreTypeRand >= TableManager.OreTypeTable[i].Probability)
                oreTypeId = i;
        for (int i = 0; i < TableManager.OreGradeTable.Count; i++)
            if (oreGradeRand >= TableManager.OreGradeTable[i].Probability)
                oreGradeId = i;

        MineData data = new MineData
        {
            MineLevelId = mineLevelId,
            OreTypeId = oreTypeId,
            OreGradeId = oreGradeId,
            OreDeposit = 0,
        };

        return data;
    }
}
