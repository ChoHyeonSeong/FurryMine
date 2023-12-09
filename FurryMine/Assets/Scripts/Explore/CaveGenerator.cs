using AutoLetterbox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    [SerializeField]
    private int _caveWidth;
    [SerializeField]
    private int _caveHeight;
    [Range(0, 100)]
    [SerializeField]
    private int _randomFillPercent;

    private const int WALL = 0;
    private const int PILLAR = 1;

    public int[,] MapRandomFill()
    {
        string seed = Time.time.ToString();
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        int[,] map = new int[_caveWidth, _caveHeight];
        for (int x = 0; x < _caveWidth; x++)
            for (int y = 0; y < _caveHeight; y++)
                map[x, y] = pseudoRandom.Next(0, 100) < _randomFillPercent ? WALL : PILLAR;
        return map;
    }

    public List<Vector2Int> GetRandomLode(int[,] map, int lodeCount)
    {
        List<Vector2Int> wallList = new List<Vector2Int>();
        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                if (map[x, y] == 0)
                    wallList.Add(new Vector2Int(x, y));

        List<Vector2Int> loadList = new List<Vector2Int>();
        for (int i = 0; i < lodeCount; i++)
        {
            int rand = UnityEngine.Random.Range(0, wallList.Count);
            loadList.Add(wallList[rand]);
            wallList.RemoveAt(rand);
        }
        return loadList;
    }
}
