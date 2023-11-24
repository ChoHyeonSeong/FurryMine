using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Miner : MonoBehaviour
{
    public static Func<Ore> RequestOre { get; set; }

    private int _price;
    private int _maxMineCount;
    private int _mineCount;
    private int _power;
    private float _mineTime;
    private float _moveSpeed;
    private float _mineAnimTime = 0.333f;
    private float _delayTime = 0.3f;
    private WaitForSeconds _mineAnimWait;
    private WaitForSeconds _mineWait;

    private MineCart _cart { get => GameManager.Inst.Cart; }
    private GameObject _target;
    private SpriteRenderer _spriter;
    private Animator _animator;
    private Rigidbody2D _rigid;
    private AIPath _aiPath;

    public void Init(int power, float moveSpeed, int maxMineCount, float mineTime, float mineAnimSpeed)
    {
        _spriter = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _aiPath = GetComponent<AIPath>();
        _animator.SetBool("Idle", true);

        _power = power;
        _moveSpeed = moveSpeed;
        _mineTime = mineTime;
        _maxMineCount = maxMineCount;
        _mineAnimWait = new WaitForSeconds(_mineAnimTime * mineAnimSpeed);
        _mineWait = new WaitForSeconds(_mineTime);

        _price = 0;
        _mineCount = _maxMineCount;
    }

    public void PlusPrice(Mineral mineral)
    {
        _price += mineral.Price;
    }


    private void Update()
    {
        if (_target == null)
        {
            Ore tempOre = RequestOre();
            if (tempOre != null)
            {
                _target = tempOre.gameObject;
                tempOre.SetMiner(this);
                MoveToTarget();
            }
        }
    }

    private void FixedUpdate()
    {
        /*        if (_target != null && !_isMining && !_isBlockMove)
                {
                    _spriter.flipX = _target.transform.position.x < _rigid.position.x;
                    Vector2 dirPos = _target.transform.position;
                    dirPos -= _rigid.position;
                    Vector2 nextPos = dirPos.normalized * Time.fixedDeltaTime * _moveSpeed;
                    _rigid.MovePosition(nextPos + _rigid.position);
                    _rigid.velocity = Vector2.zero;
                    SetAnim("Run");
                }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Consts.OreTag))
        {
            Ore ore = collision.gameObject.GetComponent<Ore>();
            if (ore.gameObject == _target)
                StartMining(ore);
        }
        else if (_mineCount == 0 && collision.gameObject.CompareTag(Consts.CartTag))
        {
            GameManager.Inst.Cart.PlusMoney(_price);
            _price = 0;
            _mineCount = _maxMineCount;
            StartCoroutine(BlockMove(_delayTime, null));
        }
    }

    private void StartMining(Ore targetOre)
    {
        _aiPath.destination = transform.position;
        StartCoroutine(Mine(targetOre));
    }

    private void InitAnimatorState()
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Idle", false);
        _animator.SetBool("Mine", false);
    }

    private void SetAnim(string param)
    {
        InitAnimatorState();
        _animator.SetBool(param, true);
    }

    private void MoveToTarget()
    {
        SetAnim("Run");
        _spriter.flipX = _target.transform.position.x < _rigid.position.x;
        _aiPath.destination = _target.transform.position;
    }

    private IEnumerator Mine(Ore ore)
    {
        SetAnim("Idle");
        yield return _mineWait;
        SetAnim("Mine");
        yield return _mineAnimWait;
        if (ore.Hit(_power))
        {
            _mineCount--;
            StartCoroutine(BlockMove(_delayTime, _mineCount > 0 ? null : _cart.gameObject));
        }
        else
        {
            StartCoroutine(Mine(ore));
        }
    }

    private IEnumerator BlockMove(float blockTime, GameObject target)
    {
        SetAnim("Idle");
        _aiPath.destination = transform.position;
        yield return new WaitForSeconds(blockTime);
        _target = target;
        if (_target != null)
            MoveToTarget();
    }
}
