using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static SaveData Save { get; private set; }

    private static string _filePath = Application.persistentDataPath + "/Resources/Save.txt";

    public static void SaveJson(SaveData save)
    {
        string jsonData = JsonUtility.ToJson(save);
        File.WriteAllText(_filePath, jsonData);
    }

    public static void LoadJson()
    {
        if (!File.Exists(_filePath))
        {
            Save = new SaveData();
            //처음부터
            return;
        }
        string jsonData = File.ReadAllText(_filePath);
        Save = JsonUtility.FromJson<SaveData>(jsonData);
    }
}