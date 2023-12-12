using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardReceiver : MonoBehaviour
{
    public static Action<EEnforce> OnUpdateEnforce { get; set; }
    public static Action<EEnforce> OnRandomEnforce { get; set; }
    public static Action<int> OnRemainCoolTime { get; set; }
    public static Action OnStartCoolTime { get; set; }
    public static Action OnEndCoolTime { get; set; }

    public int RemainCoolTime { get => _remainCoolTime; }

    private const int _coolTime = 300;
    private float _crtTime = 0;
    private bool _isCounting = false;
    private int _remainCoolTime;

    private void OnEnable()
    {
        AdManager.OnReceiveReward += RandomEnforce;
        GameApp.OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        AdManager.OnReceiveReward -= RandomEnforce;
        GameApp.OnGameStart -= GameStart;
    }

    private void Update()
    {
        if (_isCounting)
        {
            _crtTime += Time.deltaTime;
            if (_crtTime >= 1f)
            {
                _crtTime -= 1f;
                _remainCoolTime--;
                OnRemainCoolTime(_remainCoolTime);
                if (_remainCoolTime <= 0)
                {
                    OnEndCoolTime();
                    _isCounting = false;
                    _crtTime = 0;
                }
            }
        }
    }

    private void GameStart()
    {
        if(SaveManager.Save.IsAdCoolTime)
        {
            DateTime lastExitTime = DateTime.Parse(SaveManager.Save.LastExitTime);
            TimeSpan compareTime = DateTime.Now - lastExitTime;
            int remainCoolTime = _coolTime - (int)compareTime.TotalSeconds;
            if (remainCoolTime > 0)
            {
                _remainCoolTime = remainCoolTime;
                OnRemainCoolTime(_remainCoolTime);
                OnStartCoolTime();
                _isCounting = true;
            }
        }
    }

    private void RandomEnforce()
    {
        List<EEnforce> enforceList = new List<EEnforce>();
        for (int i = 0; i < EnforceManager.EnforceCount; i++)
        {
            EEnforce enforce = (EEnforce)i;
            if (EnforceManager.GetLevel(enforce) < EnforceManager.GetLimit(enforce))
                enforceList.Add(enforce);
        }
        EEnforce rand = enforceList[Random.Range(0, enforceList.Count)];
        EnforceManager.LevelUpEnforce(rand);
        OnRandomEnforce(rand);
        OnUpdateEnforce(rand);
        _remainCoolTime = _coolTime;
        OnRemainCoolTime(_remainCoolTime);
        _isCounting = true;
    }
}