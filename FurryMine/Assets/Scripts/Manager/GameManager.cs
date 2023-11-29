using UnityEngine;

public static class GameManager
{
    public static HeadMiner Player { get; private set; }
    public static MineCart Cart { get; private set; }
    public static Mine Mine { get; private set; }
    public static RewardReceiver Reward { get; private set; }


    public static void LoadCaching()
    {
        Player = Object.FindObjectOfType<HeadMiner>();
        Cart = Object.FindAnyObjectByType<MineCart>();
        Mine = Object.FindAnyObjectByType<Mine>();
        Reward = Object.FindAnyObjectByType<RewardReceiver>();
    }
}
