using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerPool : MonoBehaviour
{
    [SerializeField]
    private Miner _minerPrefab;

    private Queue<Miner> _minerQueue;

    private void Awake()
    {
        _minerQueue = new Queue<Miner>();
    }

    public Miner CreateMiner(Vector2 spawnPos)
    {
        Miner miner;
        if (_minerQueue.Count > 0)
        {
            miner = _minerQueue.Dequeue();
            miner.transform.position = spawnPos;
            miner.gameObject.SetActive(true);
        }
        else
        {
            miner = Instantiate(_minerPrefab, spawnPos, Quaternion.identity, transform);
        }
        return miner;
    }

    public void DestoryMiner(Miner miner)
    {
        miner.gameObject.SetActive(false);
        _minerQueue.Enqueue(miner);
    }
}
