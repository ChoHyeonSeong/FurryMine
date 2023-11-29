using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardReceiver : MonoBehaviour
{
    public static Action<EEnforce> OnRandEnforce { get; set; }
    public static Action<int> OnRemainCoolTime { get; set; }
    public static Action OnEndCoolTime { get; set; }

    public int RemainCoolTime { get => _remainCoolTime; }

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

    private void GameStart()
    {
        if (SaveManager.Save.RemainCoolTime > 0)
        {
            _remainCoolTime = SaveManager.Save.RemainCoolTime;
            OnRemainCoolTime(_remainCoolTime);
            StartCoroutine(CoolTimeReward(_remainCoolTime));
        }
    }

    private void RandomEnforce()
    {
        var enumArray = Enum.GetValues(typeof(EEnforce));
        List<EEnforce> enforceList = new List<EEnforce>();
        foreach (EEnforce enforce in enumArray)
        {
            enforceList.Add(enforce);
        }
        EEnforce rand = enforceList[Random.Range(0, enforceList.Count)];
        EnforceManager.LevelUpEnforce(rand);
        OnRandEnforce(rand);
        _remainCoolTime = 10;
        OnRemainCoolTime(_remainCoolTime);
        StartCoroutine(CoolTimeReward(_remainCoolTime));
    }

    private IEnumerator CoolTimeReward(int seconds)
    {
        yield return new WaitForSeconds(1);
        if (seconds <= 0)
        {
            OnEndCoolTime();
        }
        else
        {
            _remainCoolTime = seconds - 1;
            OnRemainCoolTime(_remainCoolTime);
            StartCoroutine(CoolTimeReward(_remainCoolTime));
        }
    }
}