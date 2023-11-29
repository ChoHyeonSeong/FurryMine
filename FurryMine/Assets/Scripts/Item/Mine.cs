using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public static Action<int> OnSetMineLevel { get; set; }
    public static Action<float> OnSetSubmitMineral { get; set; }
    public static Action<float> OnSetTime { get; set; }

    public int MineLevel { get => _mineLevel; }

    private int _quotaCount;
    private int _currentCount;
    private float _limitTime = 60f;
    private float _time;
    private int _mineLevel;
    private MineralSpawner _mineralSpawner;
    private OreSpawner _oreSpawner;

    private void Awake()
    {
        _mineralSpawner = FindAnyObjectByType<MineralSpawner>();
        _oreSpawner = FindAnyObjectByType<OreSpawner>();

        _currentCount = 0;
        _quotaCount = 5;
        _time = _limitTime;
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        if (_time < 0)
        {
            _time = _limitTime;
            _currentCount = 0;
            OnSetSubmitMineral(_currentCount);
        }
        OnSetTime(_time / _limitTime);
    }


    private void OnEnable()
    {
        GameApp.OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        GameApp.OnGameStart -= GameStart;
    }

    public void SubmitMineral(int count)
    {
        _currentCount += count;
        if (_currentCount >= _quotaCount)
        {
            _time = _limitTime;
            _currentCount -= _quotaCount;
            _quotaCount = (int)(_quotaCount * Consts.GoldenRatio);
            UpdateMine();
            OnSetMineLevel(_mineLevel);
        }
        OnSetSubmitMineral(_currentCount / (float)_quotaCount);
    }


    private void GameStart()
    {
        _mineLevel = SaveManager.Save.MineLevel;
        UpdateMine();
        OnSetMineLevel(_mineLevel);
    }

    private void UpdateMine()
    {
        MineEntity entity = TableManager.MineTable[_mineLevel];
        _mineralSpawner.InitMineral(entity.MineralPrice);
        _oreSpawner.InitOre(entity.OreHealth, entity.OreCount, entity.MineralCount);
    }
}
