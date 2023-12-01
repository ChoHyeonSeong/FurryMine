using System;
using UnityEngine;

public class GameApp : MonoBehaviour
{
    public static Action OnGameStart { get; set; }

    public static bool IsGameStart { get; private set; } = false;

    private static int _loadingCount = 0;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        TableManager.OnComplete += CompleteLoading;
        SaveManager.OnComplete += CompleteLoading;
        ResourceManager.OnComplete += CompleteLoading;

        TableManager.LoadTable();
        SaveManager.LoadGame();
        ResourceManager.LoadResource();
    }

    private void OnDestroy()
    {
        TableManager.OnComplete -= CompleteLoading;
        SaveManager.OnComplete -= CompleteLoading;
        ResourceManager.OnComplete -= CompleteLoading;
    }

    private void CompleteLoading()
    {
        _loadingCount--;
        if(_loadingCount <= 0)
        {
            EnforceManager.LoadEnforce();
            GameManager.LoadCaching();
            AdManager.LoadRewardedAd();
            IsGameStart = true;
            OnGameStart();
        }
    }

    public static void AddLoading(int loadingCount)
    {
        _loadingCount += loadingCount;
    }

    private void OnApplicationPause(bool pause)
    {
#if UNITY_EDITOR
#else
        if (pause)
        {
            SaveManager.SaveGame(new SaveData(
                GameManager.Cart.Money,
                GameManager.Mine.MineLevel,
                GameManager.Reward.RemainCoolTime,
                EnforceManager.GetEnforceLevelList()
                ));
        }
#endif
    }

    private void OnApplicationQuit()
    {
        TableManager.UnloadTable();
        ResourceManager.UnloadResource();
#if UNITY_EDITOR
#else
        SaveManager.SaveGame(new SaveData(
            GameManager.Cart.Money,
            GameManager.Mine.MineLevel,
            GameManager.Reward.RemainCoolTime,
            EnforceManager.GetEnforceLevelList()
            ));
#endif
    }
}
