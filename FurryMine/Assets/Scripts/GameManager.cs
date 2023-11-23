using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }

    public MineCart Cart { get; private set; }
    public HeadMiner Player { get; private set; }


    private void Awake()
    {
        Inst = this;
        Cart = FindAnyObjectByType<MineCart>();
        Player = FindAnyObjectByType<HeadMiner>();
    }
}
