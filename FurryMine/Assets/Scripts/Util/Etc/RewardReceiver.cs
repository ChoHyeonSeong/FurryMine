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
    public static Action<bool> OnStartCoolTime { get; set; }
    public static Action OnEndCoolTime { get; set; }

    public string AdDateTime { get => _adDateTime; }

    private const int _coolTime = 300;
    private float _crtTime = 0;
    private bool _isCounting = false;
    private int _remainCoolTime;
    private string _adDateTime;

    private void OnEnable()
    {
        AdManager.OnReceiveReward += RandomEnforce;
        TempAdPage.OnReceiveReward += RandomEnforce;
        AdManager.OnCompleteAdLoading += InitAdReward;
    }

    private void OnDisable()
    {
        AdManager.OnReceiveReward -= RandomEnforce;
        TempAdPage.OnReceiveReward -= RandomEnforce;
        AdManager.OnCompleteAdLoading -= InitAdReward;
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
                    _adDateTime = string.Empty;
                }
            }
        }
    }

    private void InitAdReward(bool _)
    {
        if (SaveManager.Save.AdDateTime != string.Empty)
        {
            DateTime adDateTime = DateTime.Parse(SaveManager.Save.AdDateTime);
            TimeSpan compareTime = DateTime.Now - adDateTime;
            int remainCoolTime = _coolTime - (int)compareTime.TotalSeconds;
            if (remainCoolTime > 0)
            {
                _remainCoolTime = remainCoolTime;
                OnRemainCoolTime(_remainCoolTime);
                OnStartCoolTime(true);
                _isCounting = true;
                _adDateTime = SaveManager.Save.AdDateTime;
                return;
            }
        }
        OnStartCoolTime(false);
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
        _adDateTime = DateTime.Now.ToString();
#if UNITY_EDITOR
#else
            SaveManager.SaveGame();
#endif
    }
}