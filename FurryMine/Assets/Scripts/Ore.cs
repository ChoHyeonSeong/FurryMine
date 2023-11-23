using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField]
    private GameObject _mineralPrefab;

    public Vector2 RigidPosition { get => _rigid.position; }
    private Rigidbody2D _rigid;
    private int _health = 20;
    private int _mineralCount = 5;

    // true == ±úÁü
    // false == ¾È±úÁü
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
        Vector2 dropPos;
        for (int i = 0; i < _mineralCount; i++)
        {
            dropPos = transform.position;
            dropPos.x += Random.Range(-1, 1f);
            dropPos.y += Random.Range(-1, 1f);
            Instantiate(_mineralPrefab, dropPos, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
}
