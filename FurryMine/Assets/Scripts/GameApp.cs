using System;
using UnityEngine;

public class GameApp : MonoBehaviour
{
    public static Action OnPreGameStart { get; set; }
    public static Action OnGameStart { get; set; }

    public static bool IsGameStart { get; private set; } = false;

    private static int _loadingCount = 0;

    public static void PlusLoadingCount(int count)
    {
        _loadingCount += count;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        TableManager.OnComplete += CompleteLoading;
        SaveManager.OnComplete += CompleteLoading;
        ResourceManager.OnComplete += CompleteLoading;
        AdManager.OnComplete += CompleteLoading;

        TableManager.LoadTable();
        SaveManager.LoadGame();
        ResourceManager.LoadResource();
        AdManager.LoadRewardedAd();
    }

    private void OnDestroy()
    {
        TableManager.OnComplete -= CompleteLoading;
        SaveManager.OnComplete -= CompleteLoading;
        ResourceManager.OnComplete -= CompleteLoading;
        AdManager.OnComplete -= CompleteLoading;
    }

    private void CompleteLoading()
    {
        _loadingCount--;
        if (_loadingCount <= 0)
        {
            //Debug.Log("Complete Loading");
            //Debug.Log("LoadEnforce");
            EnforceManager.LoadEnforce();
            //Debug.Log("LoadCaching");
            GameManager.LoadCaching();
            IsGameStart = true;
            OnPreGameStart();
            OnGameStart();
        }
    }

    private void OnApplicationPause(bool pause)
    {
#if UNITY_EDITOR
#else
        if (pause)
        {   
            SaveData saveGame = new SaveData();
            saveGame.Money = GameManager.Cart.Money;
            saveGame.OwnerLevel = GameManager.Player.OwnerLevel;
            saveGame.AdDateTime = GameManager.Reward.AdDateTime;
            saveGame.CurrentMineIndex = GameManager.Mine.CurrentMineIndex;
            saveGame.CurrentHeadId = GameManager.Team.HeadMinerId;
            saveGame.CurrentStaffIds = GameManager.Team.GetCurrentStaffIdList();
            saveGame.CurrentMinerEquip = GameManager.Team.CurrentMinerEquip;
            saveGame.EnforceLevels = EnforceManager.GetLevelList();
            saveGame.MineDatas = GameManager.Mine.MineDataList;
            SaveManager.SaveGame(saveGame);
        }
#endif
    }

    private void OnApplicationQuit()
    {
        TableManager.UnloadTable();
        ResourceManager.UnloadResource();
#if UNITY_EDITOR
#else
        SaveData saveGame = new SaveData();
        saveGame.Money = GameManager.Cart.Money;
        saveGame.OwnerLevel = GameManager.Player.OwnerLevel;
        saveGame.AdDateTime = GameManager.Reward.AdDateTime;
        saveGame.CurrentMineIndex = GameManager.Mine.CurrentMineIndex;
        saveGame.CurrentHeadId = GameManager.Team.HeadMinerId;
        saveGame.CurrentStaffIds = GameManager.Team.GetCurrentStaffIdList();
        saveGame.CurrentMinerEquip = GameManager.Team.CurrentMinerEquip;
        saveGame.EnforceLevels = EnforceManager.GetLevelList();
        saveGame.MineDatas = GameManager.Mine.MineDataList;
        SaveManager.SaveGame(saveGame);
#endif
    }
}
