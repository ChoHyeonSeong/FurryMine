using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public static Action OnBreakOre { get; set; }

    [SerializeField]
    private Mineral _mineralPrefab;

    public Vector2 RigidPosition { get => _rigid.position; }
    private Rigidbody2D _rigid;
    private int _health = 20;
    private int _mineralCount = 10;
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

    private void Break()
    {
        for (int i = 0; i < _mineralCount; i++)
        {
            Mineral mineral = Instantiate(_mineralPrefab, transform.position, Quaternion.identity);
            mineral.Init(_miner);
        }
        OnBreakOre();
        Destroy(gameObject);
    }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
}
