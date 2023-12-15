using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static Action OnComplete { get; set; }
    public static SaveData Save { get; private set; }

    private static string _filePath = Application.persistentDataPath + "/Save.txt";

    public static void SaveGame()
    {
        Save.Money = GameManager.Cart.Money;
        Save.OwnerLevel = GameManager.Player.OwnerLevel;
        Save.AdDateTime = GameManager.Reward.AdDateTime;
        Save.CurrentMineIndex = GameManager.Mine.CurrentMineIndex;
        Save.CurrentHeadId = GameManager.Team.HeadMinerId;
        Save.CurrentStaffIds = GameManager.Team.GetCurrentStaffIdList();
        Save.CurrentMinerEquip = new List<MinerEquip>();
        foreach (var item in GameManager.Team.CurrentMinerEquip)
        {
            Save.CurrentMinerEquip.Add(new MinerEquip(item.Key, item.Value));
        }
        Save.EnforceLevels = EnforceManager.GetLevelList();
        Save.MineDatas = GameManager.Mine.MineDataList;
        string jsonData = JsonUtility.ToJson(Save);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        string code = System.Convert.ToBase64String(bytes);
        File.WriteAllText(_filePath, code);
    }

    public static void LoadGame()
    {
        GameApp.PlusLoadingCount(1);
        if (File.Exists(_filePath))
        {
            string code = File.ReadAllText(_filePath);
            byte[] bytes = System.Convert.FromBase64String(code);
            string jsonData = System.Text.Encoding.UTF8.GetString(bytes);
            Save = JsonUtility.FromJson<SaveData>(jsonData);
            if (Save.EnforceLevels != null && Save.EnforceLevels.Count < EnforceManager.EnforceCount)
            {
                Debug.Log($"Save.EnforceLevels != null && {Save.EnforceLevels.Count} < {EnforceManager.EnforceCount}");
                for (int i = Save.EnforceLevels.Count; i < EnforceManager.EnforceCount; i++)
                {
                    Save.EnforceLevels.Add(0);
                }
            }
        }
        if (Save == null)
            Save = new SaveData();
        //Debug.Log("Load Game");
        OnComplete();
    }
}