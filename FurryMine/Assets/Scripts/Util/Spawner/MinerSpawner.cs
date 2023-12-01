using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MinerSpawner : MonoBehaviour
{

    private MinerPool _minerPool;

    private void Awake()
    {
        _minerPool = GetComponent<MinerPool>();
    }

    private void OnEnable()
    {
        GameManager.SpawnHeadMiner += GameStart;
    }

    private void OnDisable()
    {
        GameManager.SpawnHeadMiner -= GameStart;
    }

    private Miner GameStart()
    {
        return SpawnMiner(SaveManager.Save.HeadMinerId);
    }

    private Miner SpawnMiner(int id)
    {
        Miner miner = _minerPool.CreateMiner(transform.position);
        MinerEntity entity = TableManager.MinerTable[id];
        miner.Init(entity.MiningPower, entity.MiningSpeed, entity.MovingSpeed, entity.MiningCount, entity.CriticalPercent, entity.CriticalPower, ResourceManager.AnimCtrlList[entity.Id]);
        return miner;
    }

    private void CollectMiner(Miner miner)
    {
        _minerPool.DestoryMiner(miner);
    }
}
