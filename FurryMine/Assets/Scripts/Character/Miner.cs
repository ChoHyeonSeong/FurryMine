using BackEnd.Game;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class Miner : MonoBehaviour
{
    public static Func<Ore> RequestOre { get; set; }
    public Action<int> OnChangeMineralCount { get; set; }

    public Transform CameraTr { get => _cameraTr; }


    private int _miningPower;
    private float _miningSpeed;
    private int _miningCount;
    private float _movingSpeed;
    private float _criticalPercent;
    private float _criticalPower;

    [SerializeField]
    private int _finalMiningPower;
    [SerializeField]
    private float _finalMiningSpeed;
    [SerializeField]
    private float _finalMovingSpeed;
    [SerializeField]
    private float _finalCriticalPercent;
    [SerializeField]
    private float _finalCriticalPower;

    private bool _isOreTarget;
    private int _mineralCount;
    private int _price;
    private int _crtMiningCount; // crt == Current
    private float _miningTime = 1f;
    private float _miningAnimTime = 0.333f;
    private float _delayTime = 0.3f;
    private WaitForSeconds _mineAnimWait;
    private WaitForSeconds _mineWait;

    [SerializeField]
    private Transform _cameraTr;
    private MineCart _cart;
    private GameObject _target;
    private SpriteRenderer _spriter;
    private Animator _animator;
    private Rigidbody2D _rigid;
    private AIPath _aiPath;

    public void Init(int baseMiningPower, float baseMiningSpeed, float basemMovingSpeed, int baseMiningCount, float baseCriticalPercent, float baseCriticalPower, RuntimeAnimatorController animCtrl)
    {
        _spriter = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _aiPath = GetComponent<AIPath>();
        _animator.runtimeAnimatorController = animCtrl;
        _animator.SetBool("Idle", true);

        _miningPower = baseMiningPower;
        _miningSpeed = baseMiningSpeed;
        _movingSpeed = basemMovingSpeed;
        _miningCount = baseMiningCount;
        _criticalPercent = baseCriticalPercent;
        _criticalPower = baseCriticalPower;

        _isOreTarget = false;
        _price = 0;
        _mineralCount = 0;
        _target = null;
        _crtMiningCount = _miningCount;
        _cart = GameManager.Cart;
    }

    public void EnforceStat(EEnforce enforce, float enforceFigure)
    {
        switch (enforce)
        {
            case EEnforce.STAFF_MINING_POWER:
            case EEnforce.HEAD_MINING_POWER:
                _finalMiningPower = Mathf.RoundToInt((_miningPower + (int)enforceFigure) * Snack.MiningPowerBuff);
                break;
            case EEnforce.STAFF_MINING_SPEED:
            case EEnforce.HEAD_MINING_SPEED:
                _finalMiningSpeed = _miningSpeed * enforceFigure * Snack.MiningSpeedBuff;
                _animator.SetFloat("MineSpeed", _finalMiningSpeed);
                _mineAnimWait = new WaitForSeconds(_miningAnimTime / _finalMiningSpeed);
                _mineWait = new WaitForSeconds(_miningTime / _finalMiningSpeed);
                break;
            case EEnforce.STAFF_MOVING_SPEED:
            case EEnforce.HEAD_MOVING_SPEED:
                _finalMovingSpeed = _movingSpeed * enforceFigure * Snack.MovingSpeedBuff;
                _aiPath.maxSpeed = _finalMovingSpeed;
                break;
            case EEnforce.HEAD_CRITICAL_PERCENT:
                _finalCriticalPercent = _criticalPercent + enforceFigure;
                break;
            case EEnforce.HEAD_CRITICAL_POWER:
                _finalCriticalPower = _criticalPower + enforceFigure;
                break;
            default:
                Debug.Log("지정되지 않은 광부 강화입니다.");
                break;
        }
    }

    public void PlusPrice()
    {
        _mineralCount++;
        _price += MineralSpawner.MineralPrice;
        OnChangeMineralCount(_mineralCount);
    }

    public void GoToSpare()
    {
        if(_isOreTarget)
        {
            _target.GetComponent<Ore>().SetMiner(null);
        }
        else
        {
            OnChangeMineralCount(0);
            GameManager.Cart.PlusMoney(_price);
        }
    }

    private void Update()
    {
        if (_target == null)
        {
            Ore tempOre = RequestOre();
            if (tempOre != null)
            {
                Debug.Log("광석 발견");
                _target = tempOre.gameObject;
                tempOre.SetMiner(this);
                _isOreTarget = true;
                MoveToTarget();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Consts.OreTag))
        {
            Ore ore = collision.gameObject.GetComponent<Ore>();
            if (ore.gameObject == _target)
                StartMining(ore);
        }
        else if (_crtMiningCount == 0 && collision.gameObject.CompareTag(Consts.CartTag))
        {
            GameManager.Cart.PlusMoney(_price);
            GameManager.Player.SubmitMineral(_mineralCount);
            _mineralCount = 0;
            _price = 0;
            _crtMiningCount = _miningCount;
            OnChangeMineralCount(_mineralCount);
            StartCoroutine(BlockMove(_delayTime, null));
        }
    }

    private void StartMining(Ore targetOre)
    {
        Debug.Log("채광 시작");
        _aiPath.destination = transform.position;
        StartCoroutine(MineOre(targetOre));
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

    private bool CheckCritical()
    {
        return _finalCriticalPercent > Random.value;
    }

    private IEnumerator MineOre(Ore ore)
    {
        SetAnim("Idle");
        yield return _mineWait;
        SetAnim("Mine");
        yield return _mineAnimWait;
        if (ore.Hit((int)(_finalMiningPower * (CheckCritical() ? _finalCriticalPower : 1))))
        {
            _isOreTarget = false;
            _crtMiningCount--;
            StartCoroutine(BlockMove(_delayTime, _crtMiningCount > 0 ? null : _cart.gameObject));
        }
        else
        {
            StartCoroutine(MineOre(ore));
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
