using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    [SerializeField]
    private int _caveWidth;
    [SerializeField]
    private int _caveHeight;
    [SerializeField]
    private int _smoothNum;

    private int[,] _map;
    private const int ROAD = 0;
    private const int WALL = 1;
    private List<Vector2Int> _pathToLode;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GenerateMap(3);
    }

    public void GenerateMap(int lodeCount)
    {
        _map = new int[_caveWidth, _caveHeight];
        MapRandomFill(lodeCount);
        for (int i = 0; i < lodeCount; i++)
            SmoothMap();
    }

    private void MapRandomFill(int lodeCount)
    {
        // Random으로 Fill 한다음
        // 가생이 채우고
        // 시작점 (5,0)에서 각 Lode로 가는 Path 구하기
        // Smooth할때마다 LodePath를 ROAD로 채우고 Smoothing
        List<Vector2Int> map = new List<Vector2Int>();
        for (int x = 0; x < _caveWidth; x++)
            for (int y = 0; y < _caveHeight; y++)
                map.Add(new Vector2Int(x, y));
        List<Vector2Int> lodes = new List<Vector2Int>();
        for (int i = 0; i < lodeCount; i++)
        {
            int rand = Random.Range(0, map.Count);
            lodes.Add(map[rand]);
            map.RemoveAt(rand);
        }
        _pathToLode = new List<Vector2Int>();
    }

    private void SmoothMap()
    {

    }
}
