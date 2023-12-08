using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave : MonoBehaviour
{
    private int _caveWidth;
    private int _caveHeight;
    private CaveWall[,] _cave;
    private List<Vector2Int> _dirList;

    private void Awake()
    {
        _dirList = new List<Vector2Int>
        {
            new Vector2Int(1,0),
            new Vector2Int(0,1),
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
        };
        CaveWall.OnBreakCaveWall += SetHitable;
    }

    private void OnDestroy()
    {
        CaveWall.OnBreakCaveWall -= SetHitable;
    }

    public void InitCave(CaveWall[,] cave)
    {
        _cave = cave;
        _caveWidth = _cave.GetLength(0);
        _caveHeight = _cave.GetLength(1);
    }

    public void SetHitable(Vector2Int wallPos)
    {
        foreach (Vector2Int dir in _dirList)
        {
            Vector2Int tempPos = wallPos + dir;
            if (tempPos.x < _caveWidth && tempPos.y < _caveHeight && tempPos.x >= 0 && tempPos.y >= 0 && _cave[tempPos.x, tempPos.y] != null)
            {
                _cave[tempPos.x, tempPos.y].TrueHitable();
            }
        }
    }
}
