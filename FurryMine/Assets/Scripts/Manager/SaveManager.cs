using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static SaveData Save { get; private set; }

    private static string _filePath = Application.persistentDataPath + "/Save.txt";

    public static void SaveGame(SaveData save)
    {
        string jsonData = JsonUtility.ToJson(save);
        File.WriteAllText(_filePath, jsonData);
    }

    public static void LoadGame()
    {
        if (File.Exists(_filePath))
        {
            string jsonData = File.ReadAllText(_filePath);
            Save = JsonUtility.FromJson<SaveData>(jsonData);
        }
        else
        {
            Save = new SaveData();
            //처음부터
        }
    }
}