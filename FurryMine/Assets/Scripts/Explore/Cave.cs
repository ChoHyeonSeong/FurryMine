using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cave : MonoBehaviour
{
    public static Action OnFailDiscover { get; set; }
    public static Action<MineData> OnDiscoverMine { get; set; }
    public static Action OnCompleteExplore { get; set; }
    public static Action<int, int, int> OnUpdateExploreBoard { get; set; }
    public static Action<int, int> OnUpdateMiningHealth { get; set; }

    [SerializeField]
    private Vector2Int _startPos;
    [SerializeField]
    private float _padding;
    [SerializeField]
    private CaveWall _wallPrefab;
    [SerializeField]
    private GameObject _pillarPrefab;
    [SerializeField]
    private Lode _lodePrefab;
    [SerializeField]
    private int _miningHealth;
    private int _crtMiningHealth;
    private int _crtLodeCount;
    private int _lodeCount;
    private int _caveWidth;
    private int _caveHeight;
    private CaveGenerator _generator;
    private CaveWall[,] _caveWalls;
    private List<Vector2Int> _dirList;
    private List<CaveWall> _wallList;
    private List<GameObject> _pillarList;
    private List<Lode> _lodesList;
    private MineData _mineData;
    private MineLevelEntity _mineLevelEntity;

    private Queue<CaveWall> _wallPool;
    private Queue<GameObject> _pillarPool;
    private Queue<Lode> _lodePool;

    private void Awake()
    {
        _generator = GetComponent<CaveGenerator>();
        _lodesList = new List<Lode>();
        _wallList = new List<CaveWall>();
        _pillarList = new List<GameObject>();
        _wallPool = new Queue<CaveWall>();
        _pillarPool = new Queue<GameObject>();
        _lodePool = new Queue<Lode>();
        _dirList = new List<Vector2Int>
        {
            new Vector2Int(1,0),
            new Vector2Int(0,1),
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
        };
        CaveMapPanel.OnGenerateCave += InitCave;
        CaveWall.OnBreakCaveWall += SetHitable;
        CaveWall.OnHitCaveWall += MinusMiningHealth;
        Lode.OnDiscoverLode += PlusOreDeposit;
        ExplorePage.OnEndExplore += CollectCaveObject;
    }

    private void OnDestroy()
    {
        CaveMapPanel.OnGenerateCave -= InitCave;
        CaveWall.OnBreakCaveWall -= SetHitable;
        CaveWall.OnHitCaveWall -= MinusMiningHealth;
        Lode.OnDiscoverLode -= PlusOreDeposit;
        ExplorePage.OnEndExplore -= CollectCaveObject;
    }

    public void InitCave()
    {
        _mineData = MineSignature.CurrentSignature.MineData;
        _mineLevelEntity = TableManager.MineLevelTable[_mineData.MineLevelId];
        _crtMiningHealth = _miningHealth;
        _crtLodeCount = 0;
        _lodeCount = _mineLevelEntity.LodeCount;
        int[,] cave = _generator.MapRandomFill();
        cave[_startPos.x, _startPos.y] = -1;
        List<Vector2Int> lodePosList = _generator.GetRandomLode(cave, _lodeCount);
        foreach (Vector2Int lodePos in lodePosList)
        {
            Lode lode = CreateLode();
            Vector2 pos = lodePos;
            lode.transform.localPosition = pos * _padding;
            _lodesList.Add(lode);
        }

        _caveWidth = cave.GetLength(0);
        _caveHeight = cave.GetLength(1);
        _caveWalls = new CaveWall[_caveWidth, _caveHeight];
        for (int x = 0; x < _caveWidth; x++)
        {
            for (int y = 0; y < _caveHeight; y++)
            {
                // WALL
                if (cave[x, y] == 0)
                {
                    CaveWall wall = CreateWall();
                    _wallList.Add(wall);
                    wall.Init(Random.Range(1, _mineLevelEntity.Level + 1), new Vector2Int(x, y));
                    wall.transform.localPosition = new Vector2(x, y) * _padding;
                    _caveWalls[x, y] = wall;
                }
                else if (cave[x, y] == 1)
                {
                    GameObject obj = CreatePillar();
                    _pillarList.Add(obj);
                    obj.transform.localPosition = new Vector2(x, y) * _padding;
                }
            }
        }

        for (int i = 0; i < lodePosList.Count; i++)
        {
            _caveWalls[lodePosList[i].x, lodePosList[i].y].SetLode(_lodesList[i]);
        }
        SetHitable(_startPos);
        OnUpdateExploreBoard(_mineData.OreDeposit, _crtLodeCount, _lodeCount);
        OnUpdateMiningHealth(_crtMiningHealth, _miningHealth);
    }

    public void SetHitable(CaveWall wall)
    {
        SetHitable(wall.WallPos);
    }

    private void SetHitable(Vector2Int pos)
    {
        foreach (Vector2Int dir in _dirList)
        {
            Vector2Int tempPos = pos + dir;
            if (tempPos.x < _caveWidth && tempPos.y < _caveHeight && tempPos.x >= 0 && tempPos.y >= 0 && _caveWalls[tempPos.x, tempPos.y] != null)
            {
                _caveWalls[tempPos.x, tempPos.y].TrueHitable();
            }
        }
    }

    private void PlusOreDeposit()
    {
        _crtLodeCount++;
        _mineData.OreDeposit += _mineLevelEntity.OrePerLode;
        OnUpdateExploreBoard(_mineData.OreDeposit, _crtLodeCount, _lodeCount);
    }

    private void MinusMiningHealth()
    {
        _crtMiningHealth--;
        OnUpdateMiningHealth(_crtMiningHealth, _miningHealth);
        if (_crtMiningHealth <= 0 || _crtLodeCount >= _lodeCount)
        {
            if (_mineData.OreDeposit > 0)
                OnDiscoverMine(_mineData);
            else
                OnFailDiscover();
            OnCompleteExplore();
        }
    }

    private void CollectCaveObject()
    {
        foreach (CaveWall wall in _wallList)
        {
            wall.gameObject.SetActive(false);
            _wallPool.Enqueue(wall);
        }

        foreach (GameObject pillar in _pillarList)
        {
            pillar.SetActive(false);
            _pillarPool.Enqueue(pillar);
        }

        foreach (Lode lode in _lodesList)
        {
            lode.gameObject.SetActive(false);
            _lodePool.Enqueue(lode);
        }

        _wallList.Clear();
        _pillarList.Clear();
        _lodesList.Clear();
    }


    private CaveWall CreateWall()
    {
        CaveWall wall;
        if (_wallPool.Count > 0)
        {
            wall = _wallPool.Dequeue();
            wall.gameObject.SetActive(true);
        }
        else
        {
            wall = Instantiate(_wallPrefab, transform);
        }
        return wall;
    }

    private GameObject CreatePillar()
    {
        GameObject pillar;
        if (_pillarPool.Count > 0)
        {
            pillar = _pillarPool.Dequeue();
            pillar.gameObject.SetActive(true);
        }
        else
        {
            pillar = Instantiate(_pillarPrefab, transform);
        }
        return pillar;
    }

    private Lode CreateLode()
    {
        Lode lode;
        if (_lodePool.Count > 0)
        {
            lode = _lodePool.Dequeue();
            lode.OffShine();
            lode.gameObject.SetActive(true);
        }
        else
        {
            lode = Instantiate(_lodePrefab, transform);
        }
        return lode;
    }
}
