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

    private int _level = 1;

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
    }
}
