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
        Miner head = _minerPool.CreateMiner();
        MinerEntity entity = TableManager.MinerTable[SaveManager.Save.HeadMinerId];
        head.Init(entity.MiningPower, entity.MiningSpeed, entity.MovingSpeed, entity.MiningCount, entity.CriticalPercent, entity.CriticalPower, ResourceManager.AnimCtrlList[entity.Id]);
        return head;
    }

}
