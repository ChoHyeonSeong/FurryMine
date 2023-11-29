using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static SaveData Save { get; private set; }

    private static string _filePath = Application.persistentDataPath + "/Save.txt";

    public static void SaveGame(SaveData save)
    {
        string jsonData = JsonUtility.ToJson(save);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        string code = System.Convert.ToBase64String(bytes);
        File.WriteAllText(_filePath, code);
    }

    public static void LoadGame()
    {
        if (File.Exists(_filePath))
        {
            string code = File.ReadAllText(_filePath);
            byte[] bytes = System.Convert.FromBase64String(code);
            string jsonData = System.Text.Encoding.UTF8.GetString(bytes);
            Save = JsonUtility.FromJson<SaveData>(jsonData);
        }
        else
        {
            Save = new SaveData();
            //처음부터
        }
    }
}