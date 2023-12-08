using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private MineSignature _mineSignaturePrefab;

    private List<MineSignature> _mineSignatureList;

    public enum DrawMode { NoiseMap, ColorMap };
    public DrawMode _drawMode;

    public int _mapWidth;
    public int _mapHeight;
    public float _noiseScale;

    public int _octaves;
    [Range(0, 1)]
    public float _persistance;
    public float _lacunarity;

    public int _seed;
    public Vector2 _offset;

    public TerrainType[] _regions;

    private MineMapDisplay _display;
    private Color[] _colorMap;
    private float[,] _noiseMap;


    private void Awake()
    {
        _display = GetComponent<MineMapDisplay>();
        _mineSignatureList = new List<MineSignature>();
    }

    public void GenerateMap()
    {
        _seed = UnityEngine.Random.Range(0, int.MaxValue);
        _colorMap = new Color[_mapWidth * _mapHeight];
        _noiseMap = Noise.GenerateNoiseMap(_mapWidth, _mapHeight, _seed, _noiseScale, _octaves, _persistance, _lacunarity, _offset);
        for (int y = 0; y < _mapHeight; y++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                float currentHeight = _noiseMap[x, y];
                for (int i = 0; i < _regions.Length; i++)
                {
                    if (currentHeight <= _regions[i].Height)
                    {
                        _colorMap[y * _mapWidth + x] = _regions[i].Color;
                        break;
                    }
                }
            }
        }

        if (_drawMode == DrawMode.NoiseMap)
            _display.DrawTexture(TextureGenerator.TextureFromHeightMap(_noiseMap));
        else if (_drawMode == DrawMode.ColorMap)
            _display.DrawTexture(TextureGenerator.TextureFromColorMap(_colorMap, _mapWidth, _mapHeight));
    }

    public void GenerateMineSignature(int signatureCount)
    {
        List<Vector2> signatureList = new List<Vector2>();

        int maxX = (int)(_mapWidth * 0.9f);
        int minX = (int)(_mapWidth * 0.1f);
        int maxY = (int)(_mapHeight * 0.9f);
        int minY = (int)(_mapHeight * 0.1f);

        for (int y = 0; y < _mapHeight; y++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                float currentHeight = _noiseMap[x, y];
                if (currentHeight > 0.55f && currentHeight <= 0.9f)
                {
                    if (maxX > x && minX < x && maxY > y && minY < y)
                    {
                        signatureList.Add(new Vector2(x - 50, y - 50) * 10);
                    }
                }
            }
        }

        for (int i = _mineSignatureList.Count; i < signatureCount; i++)
        {
            _mineSignatureList.Add(Instantiate(_mineSignaturePrefab, transform));
        }

        for (int i = 0; i < signatureCount; i++)
        {
            int rand = UnityEngine.Random.Range(0, signatureList.Count);
            _mineSignatureList[i].transform.localPosition = signatureList[rand];
            _mineSignatureList[i].InitMineData(MineGenerator.GenerateMine());
            signatureList.RemoveAt(rand);
        }
    }

    private void OnValidate()
    {
        if (_mapWidth < 1)
            _mapWidth = 1;
        if (_mapHeight < 1)
            _mapHeight = 1;
        if (_lacunarity < 1)
            _lacunarity = 1;
        if (_octaves < 0)
            _octaves = 0;
    }
}

[Serializable]
public struct TerrainType
{
    public string Name;
    public float Height;
    public Color Color;
}
