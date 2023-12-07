using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoader : MonoBehaviour
{
    private MapGenerator _generator;
    int loading = 6;
    private void Awake()
    {
        _generator = FindAnyObjectByType<MapGenerator>();
        TableManager.OnComplete += Complete;
    }

    private void Start()
    {
        TableManager.TestLoad();
    }

    private void Complete()
    {
        loading--;
        if (loading == 0)
        {
            _generator.GenerateMap();
            _generator.GenerateMinePosition();
        }
    }
}
