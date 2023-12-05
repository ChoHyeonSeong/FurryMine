using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip
{
    private EquipEntity _equipEntity;

    public int EquipId { get => _equipEntity.Id; }
    public string EquipName { get => _equipEntity.Name; }
    public int FinalMiningPower { get; private set; }
    public float FinalMiningSpeed { get; private set; }
    public float FinalMovingSpeed { get; private set; }
    public int FinalMiningCount { get; private set; }
    public float FinalCriticalPercent { get; private set; }
    public float FinalCriticalPower { get; private set; }

    public Equip(EquipEntity entity)
    {
        _equipEntity = entity;

        FinalMiningPower = entity.MiningPower;
        FinalMiningSpeed = entity.MiningSpeed;
        FinalMovingSpeed = entity.MovingSpeed;
        FinalMiningCount = entity.MiningCount;
        FinalCriticalPercent = entity.CriticalPercent;
        FinalCriticalPower = entity.CriticalPower;
    }
}
