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

    private int _quotaExp;
    private int _currentExp;
    private float _limitTime = 60f;
    private float _time;
    private int _ownerLevel;
    // 가지고 있는 광부목록
    private List<int> _minerIdList;
    // 가지고 있는 광산목록
    private List<MineData> _mineDataList;

    private void Awake()
    {
        _quotaExp = 5;
        _currentExp = 0;
        _time = _limitTime;
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
        _minerIdList = SaveManager.Save.MinerIds.ToList();
        _mineDataList = SaveManager.Save.MineDatas.ToList();
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
