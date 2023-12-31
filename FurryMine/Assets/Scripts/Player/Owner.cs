using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Owner : MonoBehaviour
{
    public static Action<int> OnSetOwnerLevel { get; set; }
    public static Action<float> OnSetSubmitMineral { get; set; }
    public static Action<float> OnSetTime { get; set; }

    public int OwnerLevel { get => _ownerLevel; }

    private int _quotaExp;
    private int _currentExp;
    private float _limitTime = 60f;
    private float _time;
    private int _ownerLevel;

    private void Awake()
    {
        _quotaExp = 5;
        _currentExp = 0;
        _time = _limitTime;
    }

    private void Update()
    {
        if (GameApp.IsGameStart)
        {
            _time -= Time.deltaTime;
            if (_time < 0)
            {
                _time = _limitTime;
                _currentExp = 0;
                OnSetSubmitMineral(_currentExp);
            }
            OnSetTime(_time / _limitTime);
        }
    }

    private void OnEnable()
    {
        GameApp.OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        GameApp.OnGameStart -= GameStart;
    }

    private void GameStart()
    {
        _ownerLevel = SaveManager.Save.OwnerLevel;
        for (int i = 0; i < _ownerLevel; i++)
        {
            _quotaExp = (int)(_quotaExp * Consts.GoldenRatio);
        }
    }

    public void SubmitMineral(int count)
    {
        _currentExp += count;
        if (_currentExp >= _quotaExp)
        {
            _ownerLevel++;
            _time = _limitTime;
            _currentExp -= _quotaExp;
            _quotaExp = (int)(_quotaExp * Consts.GoldenRatio);
            OnSetOwnerLevel(_ownerLevel);
        }
        OnSetSubmitMineral(_currentExp / (float)_quotaExp);
    }
}
