using AutoLetterbox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    [SerializeField]
    private CaveWall _wallPrefab;

    [SerializeField]
    private float _wallPadding;

    [SerializeField]
    private Vector2Int _startPos;

    [SerializeField]
    private int _caveWidth;
    [SerializeField]
    private int _caveHeight;
    [Range(0, 100)]
    [SerializeField]
    private int _randomFillPercent;

    private int[,] _map;
    private string _seed;
    private const int ROAD = 0;
    private const int WALL = 1;
    private List<GameObject> _obejcts;
    private List<Vector2Int> _roadList;
    private List<Vector2Int> _loadList;
    private MineLevelEntity _mineLevelEntity;
    private Cave _cave;

    private void Awake()
    {
        _cave = GetComponent<Cave>();
        _map = new int[_caveWidth, _caveHeight];
        _startPos = new Vector2Int(5, 0);
        _obejcts = new List<GameObject>();
    }

    public void GenerateMap(int mineLevelId)
    {
        _mineLevelEntity = TableManager.MineLevelTable[mineLevelId];
        for (int i = 0; i < _obejcts.Count; i++)
            Destroy(_obejcts[i]);
        _obejcts.Clear();
        MapRandomFill();
        GetRandomLode();
        _cave.InitCave(SetWallAndRoad());
        _cave.SetHitable(_startPos);
    }

    private void MapRandomFill()
    {
        _seed = Time.time.ToString(); //시드

        System.Random pseudoRandom = new System.Random(_seed.GetHashCode()); //시드로 부터 의사 난수 생성
        _roadList = new List<Vector2Int>();

        // Random으로 Fill 한다음
        // 가생이 채우고
        for (int x = 0; x < _caveWidth; x++)
        {
            for (int y = 0; y < _caveHeight; y++)
            {
                bool isWall = (pseudoRandom.Next(0, 100) < _randomFillPercent);
                _map[x, y] = isWall ? WALL : ROAD;
                if (!isWall) _roadList.Add(new Vector2Int(x, y));
            }
        }
    }

    private void GetRandomLode()
    {
        _loadList = new List<Vector2Int>();
        if (_roadList.Contains(_startPos))
            _roadList.Remove(_startPos);

        for (int i = 0; i < _mineLevelEntity.LodeCount; i++)
        {
            int rand = UnityEngine.Random.Range(0, _roadList.Count);
            _loadList.Add(_roadList[rand]);
            _roadList.RemoveAt(rand);
        }
    }

    private CaveWall[,] SetWallAndRoad()
    {
        CaveWall[,] caveWalls = new CaveWall[_caveWidth, _caveHeight];
        Vector2 pos = Vector2Int.zero;
        for (int x = 0; x < _caveWidth; x++)
        {
            for (int y = 0; y < _caveHeight; y++)
            {
                if (x == _startPos.x && y == _startPos.y)
                    continue;
                pos.Set(x, y);
                CaveWall wall = Instantiate(_wallPrefab, transform);
                wall.transform.localPosition = pos * _wallPadding;
                wall.Init(UnityEngine.Random.Range(1, _mineLevelEntity.Level + 1), new Vector2Int(x, y));
                caveWalls[x, y] = wall;
            }
        }
        return caveWalls;
    }
}
