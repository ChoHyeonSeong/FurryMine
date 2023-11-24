using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public static Action<Ore> OnBreakOre { get; set; }

    public int MineralCount { get => _mineralCount; }

    public Miner CurrentMiner { get => _miner; }

    public Vector2 RigidPosition { get => _rigid.position; }
    private Rigidbody2D _rigid;
    private int _health;
    private int _mineralCount;
    private Miner _miner;

    // true == ±úÁü
    // false == ¾È±úÁü

    public void SetMiner(Miner miner)
    {
        _miner = miner;
    }

    public bool Hit(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Break();
            return true;
        }
        return false;
    }

    public void Init()
    {
        _health = 20;
        _mineralCount = 10;
        _miner = null;
    }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Break()
    {
        OnBreakOre(this);
    }
}
