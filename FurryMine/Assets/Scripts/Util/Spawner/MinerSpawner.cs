using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MinerSpawner : MonoBehaviour
{

    private MinerPool _minerPool;

    public Miner SpawnMiner(int id)
    {
        Miner miner = _minerPool.CreateMiner(transform.position);
        MinerEntity entity = TableManager.MinerTable[id];
        miner.Init(entity.MiningPower, entity.MiningSpeed, entity.MovingSpeed, entity.MiningCount, entity.CriticalPercent, entity.CriticalPower, ResourceManager.AnimCtrlList[entity.Id]);
        return miner;
    }

    private void Awake()
    {
        _minerPool = GetComponent<MinerPool>();
    }

    private void CollectMiner(Miner miner)
    {
        _minerPool.DestoryMiner(miner);
    }
}
