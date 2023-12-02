using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public static Action<bool, string, Vector2> OnHitText { get; set; }
    public static Action<Ore> OnBreakOre { get; set; }

    public Miner CurrentMiner { get => _miner; }

    private int _health;
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
        OnHitText(false, damage.ToString(), transform.position);
        if (_health <= 0)
        {
            Break();
            return true;
        }
        return false;
    }

    public void Init(int health)
    {
        _health = health;
        _miner = null;
    }

    private void Break()
    {
        OnBreakOre(this);
    }
}
