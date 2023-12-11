using UnityEngine;

public class MinerSpawner : MonoBehaviour
{

    private MinerPool _minerPool;

    public Miner SpawnMiner(int id)
    {
        Miner miner = _minerPool.CreateMiner(transform.position);
        miner.Init(TableManager.MinerTable[id], ResourceManager.AnimCtrlList[id]);
        return miner;
    }

    public void CollectMiner(Miner miner)
    {
        miner.GoToSpare();
        _minerPool.DestoryMiner(miner);
    }

    private void Awake()
    {
        _minerPool = GetComponent<MinerPool>();
    }
}
