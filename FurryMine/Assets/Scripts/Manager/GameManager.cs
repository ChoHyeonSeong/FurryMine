using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class GameManager
{
    public static Owner Player { get; private set; }
    public static MinerTeam Team { get; private set; }
    public static MineCart Cart { get; private set; }
    public static Mine Mine { get; private set; }
    public static RewardReceiver Reward { get; private set; }


    public static void LoadCaching()
    {
        Player = Object.FindObjectOfType<Owner>();
        Team = Object.FindObjectOfType<MinerTeam>();
        Cart = Object.FindAnyObjectByType<MineCart>();
        Mine = Object.FindAnyObjectByType<Mine>();
        Reward = Object.FindAnyObjectByType<RewardReceiver>();
    }
}
