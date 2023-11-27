using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public static Action<bool, string, Vector2> OnHitText;
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
        OnHitText(false, damage.ToString(), transform.position);
        if (_health <= 0)
        {
            Break();
            return true;
        }
        return false;
    }

    public void Init(int health, int mineralCount)
    {
        _health = health;
        _mineralCount = mineralCount;
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
