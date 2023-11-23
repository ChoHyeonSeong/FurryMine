using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMiner : Miner
{
    private void Awake()
    {
        Init(10, 5, 1, 0.5f, 1);
    }
}
