using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MinerEntity
{
    public int Id;
    public int MiningPower;
    public float MiningSpeed;
    public float MovingSpeed;
    public int MiningCount;
    public float CriticalPercent;
    public float CriticalPower;
    public string Rank;
    public string Name;
    public string Description;
}
