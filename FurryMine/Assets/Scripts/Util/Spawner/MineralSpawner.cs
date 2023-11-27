using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralSpawner : MonoBehaviour
{
    private MineralPool _mineralPool;

    private void Awake()
    {
        _mineralPool = GetComponent<MineralPool>();
    }

    private void OnEnable()
    {
        Ore.OnBreakOre += SpawnMineral;
        Mineral.OnPickMineral += CollectMineral;
    }

    private void OnDisable()
    {
        Ore.OnBreakOre -= SpawnMineral;
        Mineral.OnPickMineral -= CollectMineral;
    }

    private void CollectMineral(Mineral mineral)
    {
        _mineralPool.DestroyMineral(mineral);
    }

    private void SpawnMineral(Ore ore)
    {
        for (int i = 0; i < ore.MineralCount; i++)
        {
            Mineral mineral = _mineralPool.CreateMineral(ore.transform.position);
            mineral.Init(ore.CurrentMiner);
        }
    }
}
