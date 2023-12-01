using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

public class BlankPool : MonoBehaviour
{
    public List<Blank> BlankList { get; private set; }

    [SerializeField]
    private Transform _worldTr;

    [SerializeField]
    private Blank _blankPrefab;

    [SerializeField]
    private Tilemap _groundMap;


    private float _scaleValue;
    private float _positionDiffX = 0.5f;
    private float _positionDiffY = 1.5f;

    private void Awake()
    {
        _scaleValue = _worldTr.lossyScale.x;
        BlankList = new List<Blank>();
        foreach (Vector3Int pos in _groundMap.cellBounds.allPositionsWithin)
        {
            if (_groundMap.HasTile(pos))
            {
                Vector3 tilePos = pos;
                tilePos.x += _positionDiffX;
                tilePos.y += _positionDiffY;
                tilePos *= _scaleValue;
                BlankList.Add(Instantiate(_blankPrefab, tilePos, Quaternion.identity, transform));
            }
        }
    }
}
