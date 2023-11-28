using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action<int> OnLevelUp { get; set; }
    public static GameManager Inst { get; private set; }

    public MineCart Cart { get; private set; }
    public HeadMiner Player { get; private set; }

    private int _level = 0;
    private EnforceManager _enforceManager;

    public void LevelUp()
    {
        _level++;
        OnLevelUp(_level);
    }


    private void Awake()
    {
        Inst = this;
        Cart = FindAnyObjectByType<MineCart>();
        Player = FindAnyObjectByType<HeadMiner>();
        _enforceManager = FindAnyObjectByType<EnforceManager>();
        DataManager.LoadData();
        SaveManager.LoadJson();
        _enforceManager.Init();
    }


    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
#else
        var enumArray = Enum.GetValues(typeof(EEnforce));
        List<int> enforceLevels = new List<int>();
        foreach (EEnforce enforce in enumArray)
        {
            enforceLevels.Add(_enforceManager.LevelDict[enforce]);
        }
        SaveManager.SaveJson(new SaveData(Cart.Money, _level, enforceLevels));
#endif
    }
}
