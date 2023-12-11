using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int _signatureCount;
    [SerializeField]
    private MineSignature _mineSignaturePrefab;

    private List<MineSignature> _mineSignatureList;

    public enum DrawMode { NoiseMap, ColorMap };
    public DrawMode Mode;

    public int MapWidth;
    public int MapHeight;
    public int RequireMoney;
    public float NoiseScale;

    public int Octaves;
    [Range(0, 1)]
    public float Persistance;
    public float Lacunarity;

    public int Seed;
    public Vector2 Offset;

    public TerrainType[] Regions;

    private MineMapDisplay _display;
    private Color[] _colorMap;
    private float[,] _noiseMap;


    private void Awake()
    {
        _display = GetComponent<MineMapDisplay>();
        _mineSignatureList = new List<MineSignature>();

        for (int i = 0; i < _signatureCount; i++)
        {
            _mineSignatureList.Add(Instantiate(_mineSignaturePrefab, transform));
        }
    }

    public void GenerateMap()
    {
        Seed = UnityEngine.Random.Range(0, int.MaxValue);
        _colorMap = new Color[MapWidth * MapHeight];
        _noiseMap = Noise.GenerateNoiseMap(MapWidth, MapHeight, Seed, NoiseScale, Octaves, Persistance, Lacunarity, Offset);
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                float currentHeight = _noiseMap[x, y];
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (currentHeight <= Regions[i].Height)
                    {
                        _colorMap[y * MapWidth + x] = Regions[i].Color;
                        break;
                    }
                }
            }
        }

        if (Mode == DrawMode.NoiseMap)
            _display.DrawTexture(TextureGenerator.TextureFromHeightMap(_noiseMap));
        else if (Mode == DrawMode.ColorMap)
            _display.DrawTexture(TextureGenerator.TextureFromColorMap(_colorMap, MapWidth, MapHeight));
    }

    public void GenerateMineSignature()
    {
        List<Vector2> signatureList = new List<Vector2>();

        int maxX = (int)(MapWidth * 0.9f);
        int minX = (int)(MapWidth * 0.1f);
        int maxY = (int)(MapHeight * 0.9f);
        int minY = (int)(MapHeight * 0.1f);

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
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

        for (int i = 0; i < _signatureCount; i++)
        {
            int rand = UnityEngine.Random.Range(0, signatureList.Count);
            _mineSignatureList[i].transform.localPosition = signatureList[rand];
            _mineSignatureList[i].InitMineData(MineGenerator.GenerateMine());
            signatureList.RemoveAt(rand);
        }
    }

    private void OnValidate()
    {
        if (MapWidth < 1)
            MapWidth = 1;
        if (MapHeight < 1)
            MapHeight = 1;
        if (Lacunarity < 1)
            Lacunarity = 1;
        if (Octaves < 0)
            Octaves = 0;
    }
}

[Serializable]
public struct TerrainType
{
    public string Name;
    public float Height;
    public Color Color;
}
