using System;
using UnityEngine;

public class GameApp : MonoBehaviour
{
    public static Action OnGameStart { get; set; }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        TableManager.LoadTable();
        SaveManager.LoadGame();
        EnforceManager.LoadEnforce();
        GameManager.LoadCaching();
        AdManager.LoadRewardedAd();
    }

    private void Start()
    {
        OnGameStart();
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
