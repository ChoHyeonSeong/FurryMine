using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OreSpawner : MonoBehaviour
{
    private bool _isSpawning = false;
    private int _mineralCount;
    private int _oreHealth;
    private int _maxOreCount;
    private int _oreCount = 0;
    private float _respawnTime = 1f;
    private OrePool _orePool;
    private BlankPool _blankPool;
    private WaitForSeconds _respawnWait;
    private Queue<Ore> _newOreQueue;

    private void Awake()
    {
        _blankPool = FindAnyObjectByType<BlankPool>();
        _orePool = GetComponent<OrePool>();
        _respawnWait = new WaitForSeconds(_respawnTime);
        _newOreQueue = new Queue<Ore>();
    }

    private void Start()
    {
        InitOreLevel(SaveManager.Save.MineLevel);
    }

    private void Update()
    {
        if (!_isSpawning && _oreCount < _maxOreCount)
        {
            _isSpawning = true;
            StartCoroutine(SpawnOre());
        }
    }

    private void OnEnable()
    {
        Miner.RequestOre += ResponseOre;
        Ore.OnBreakOre += CollectOre;
        GameManager.OnLevelUp += InitOreLevel;
    }

    private void OnDisable()
    {
        Miner.RequestOre -= ResponseOre;
        Ore.OnBreakOre -= CollectOre;
        GameManager.OnLevelUp -= InitOreLevel;
    }

    private Ore ResponseOre()
    {
        return _newOreQueue.Count > 0 ? _newOreQueue.Dequeue() : null;
    }

    private void CollectOre(Ore ore)
    {
        _oreCount--;
        _orePool.DestroyOre(ore);
        AstarPath.active.Scan();
    }

    private IEnumerator SpawnOre()
    {
        yield return _respawnWait;
        _isSpawning = false;
        _oreCount++;
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
        ore.Init(_oreHealth, _mineralCount);
        _newOreQueue.Enqueue(ore);
        AstarPath.active.Scan();
    }

    private void InitOreLevel(int level)
    {
        MineEntity entity = DataManager.MineDict[level];
        _oreHealth = entity.OreHealth;
        _maxOreCount = entity.OreCount;
        _mineralCount = entity.MineralCount;
    }
}
