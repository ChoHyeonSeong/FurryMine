using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class OreSpawner : MonoBehaviour
{
    public static int MineralCount { get; private set; }

    public Action OnSpawnedOre { get; set; }
    public Func<bool> IsSpawnable { get; set; }
    public Action OnCollectOre { get; set; }

    private int _oreHealth;
    private float _respawnTime;
    private int _oreCount;

    private bool _isSpawning = false;
    private int _currentOreCount = 0;

    private OrePool _orePool;
    private BlankPool _blankPool;

    private WaitForSeconds _respawnWait;
    private Queue<Ore> _newOreQueue;

    private void Awake()
    {
        _blankPool = FindAnyObjectByType<BlankPool>();
        _orePool = GetComponent<OrePool>();
        _newOreQueue = new Queue<Ore>();
    }

    private void Update()
    {
        if (GameApp.IsGameStart && IsSpawnable())
        {
            if (!_isSpawning && _currentOreCount < _oreCount)
            {
                _isSpawning = true;
                StartCoroutine(SpawnOre());
            }
        }
    }

    private void OnEnable()
    {
        Miner.RequestOre += ResponseOre;
        Ore.OnBreakOre += CollectOre;
        Ore.OnSetMinerNull += ReIdleOre;
    }

    private void OnDisable()
    {
        Miner.RequestOre -= ResponseOre;
        Ore.OnBreakOre -= CollectOre;
        Ore.OnSetMinerNull -= ReIdleOre;
    }

    private Ore ResponseOre()
    {
        return _newOreQueue.Count > 0 ? _newOreQueue.Dequeue() : null;
    }

    private void CollectOre(Ore ore)
    {
        _currentOreCount--;
        _orePool.DestroyOre(ore);
        OnCollectOre();
        AstarPath.active.Scan();
    }

    private void ReIdleOre(Ore ore)
    {
        _newOreQueue.Enqueue(ore);
    }

    private IEnumerator SpawnOre()
    {
        yield return _respawnWait;
        _isSpawning = false;
        _currentOreCount++;
        List<Vector2> candidate = new List<Vector2>();
        foreach (Blank blank in _blankPool.BlankList)
        {
            if (blank.ObjectCount == 0)
            {
                candidate.Add(blank.transform.position);
            }
        }
        Vector2 spawnPos = candidate[Random.Range(0, candidate.Count)];
        Ore ore = _orePool.CreateOre(spawnPos);
        ore.Init(_oreHealth);
        _newOreQueue.Enqueue(ore);
        AstarPath.active.Scan();
    }

    public void SetOreHealth(int oreHealth)
    {
        _oreHealth = oreHealth;
    }

    public void SetRespawnTime(float respawnTime)
    {
        _respawnTime = respawnTime;
        _respawnWait = new WaitForSeconds(_respawnTime);
    }

    public void SetOreCount(int oreCount)
    {
        _oreCount = oreCount;
    }

    public void SetMineralCount(int mineralCount)
    {
        MineralCount = mineralCount;
    }
}
