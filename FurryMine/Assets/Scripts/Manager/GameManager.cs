using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class GameManager
{
    public static Miner Player { get; private set; }
    public static MineCart Cart { get; private set; }
    public static Mine Mine { get; private set; }
    public static RewardReceiver Reward { get; private set; }


    public static void LoadCaching()
    {
        Cart = Object.FindAnyObjectByType<MineCart>();
        Mine = Object.FindAnyObjectByType<Mine>();
        Reward = Object.FindAnyObjectByType<RewardReceiver>();
    }
}
