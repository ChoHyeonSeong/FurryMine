using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCart : MonoBehaviour
{
    public static Action<bool, string, Vector2> OnPlusText;
    public static Action<int> OnChangeMoney { get; set; }
    public static Action<float> OnChangeCount { get; set; }
    public static Action<float> OnChangeTime { get; set; }

    private int _money = 0;

    private int _quotaCount;
    private int _currentCount;
    private float _limitTime = 60f;
    private float _time;

    private void Awake()
    {
        _currentCount = 0;
        _quotaCount = 5;
        _time = _limitTime;
    }

    private void Start()
    {
        PlusMoney(1000);
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        if (_time < 0)
        {
            _time = _limitTime;
            _currentCount = 0;
            OnChangeCount(0);
        }
        OnChangeTime(_time / _limitTime);
    }

    private void OnEnable()
    {
        GameManager.OnLevelUp += InitTime;
    }

    private void OnDisable()
    {
        GameManager.OnLevelUp -= InitTime;
    }

    private void InitTime(int _)
    {
        _time = _limitTime;
    }

    public void PlusMoney(int price)
    {
        _money += price;
        OnChangeMoney(_money);
        OnPlusText(true, $"+{price}G", transform.position);
    }

    public bool MinusMoney(int price)
    {
        if (price <= _money)
        {
            _money -= price;
            OnChangeMoney(_money);
            return true;
        }
        return false;
    }

    public void PlusCount(int count)
    {
        _currentCount += count;
        if (_currentCount >= _quotaCount)
        {
            _currentCount -= _quotaCount;
            _quotaCount = (int)(_quotaCount * Consts.GoldenRatio);
            GameManager.Inst.LevelUp();
        }
        OnChangeCount(_currentCount / (float)_quotaCount);
    }
}
