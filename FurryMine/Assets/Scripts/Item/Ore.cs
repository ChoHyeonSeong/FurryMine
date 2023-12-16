using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public static Action<bool, string, Vector2> OnHitText { get; set; }

    public static Action<Ore> OnPreBreakOre { get; set; }
    public static Action<Ore> OnBreakOre { get; set; }

    public static Action<Ore> OnSetMinerNull { get; set; }

    public Miner CurrentMiner { get => _miner; }

    private int _health;
    private Miner _miner;

    // true == ±úÁü
    // false == ¾È±úÁü

    public void SetMiner(Miner miner)
    {
        _miner = miner;
        if(miner == null)
        {
            OnSetMinerNull(this);
        }
    }

    public void Hit(int damage)
    {
        _health -= damage;  
        OnHitText(false, damage.ToString(), transform.position);
        if (_health <= 0)
        {
            Break();
        }
    }

    public void Init(int health)
    {
        _health = health;
        _miner = null;
    }

    private void Break()
    {
        _miner.BreakOre();
        OnPreBreakOre(this);
        OnBreakOre(this);
    }
}
