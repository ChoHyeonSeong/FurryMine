using Pathfinding;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Miner : MonoBehaviour
{
    public static Func<Ore> RequestOre { get; set; }
    public Action<int> OnChangeMineralCount { get; set; }

    public Transform CameraTr { get => _cameraTr; }

    public int EquipId { get => _equip == null ? -1 : _equip.EquipId; }

    public int CurrentMiningCount { get => _crtMiningCount; }

    public bool IsEqualTarget { get => _isEqualTarget; }

    public Ore TargetOre { get => _targetOre; }

    public string MinerName { get=>_minerEntity.Name; }

    public WaitForSeconds MineAnimWait { get => _mineAnimWait; }
    public WaitForSeconds MineWait { get => _mineWait; }

    private MinerEntity _minerEntity;

    private Equip _equip;

    [SerializeField]
    private int _finalMiningPower;
    [SerializeField]
    private float _finalMiningSpeed;
    [SerializeField]
    private float _finalMovingSpeed;
    [SerializeField]
    private int _finalMiningCount;
    [SerializeField]
    private float _finalCriticalPercent;
    [SerializeField]
    private float _finalCriticalPower;

    private bool _isEqualTarget;
    private bool _haveEquip;
    private int _mineralCount;
    private int _price;
    private int _crtMiningCount; // crt == Current
    private float _miningTime = 1f;
    private float _miningAnimTime = 0.333f;
    private WaitForSeconds _mineAnimWait;
    private WaitForSeconds _mineWait;

    [SerializeField]
    private Transform _cameraTr;
    private MineCart _cart;
    private Owner _player;
    private Ore _targetOre;
    private GameObject _target;
    private SpriteRenderer _spriter;
    private Animator _animator;
    private Rigidbody2D _rigid;
    private AIPath _aiPath;
    private MinerStateMachine _fsm;


    public void Init(MinerEntity minerEntity, RuntimeAnimatorController animCtrl)
    {
        _animator.runtimeAnimatorController = animCtrl;
        _animator.SetBool("Idle", true);

        _minerEntity = minerEntity;

        _haveEquip = false;
        _price = 0;
        _mineralCount = 0;
        _target = null;
        _finalMiningCount = _minerEntity.MiningCount;
        _crtMiningCount = _finalMiningCount;
        _aiPath.destination = transform.position;
        _fsm.PlayState();
        _fsm.ChangeState(EMinerState.IDLE);
    }

    public void PutOnEquip(Equip equip)
    {
        _haveEquip = true;
        _equip = equip;
        _finalMiningCount = _minerEntity.MiningCount + _equip.FinalMiningCount;
    }

    public void TakeOffEquip()
    {
        _haveEquip = false;
        _equip = null;
        _finalMiningCount = _minerEntity.MiningCount;
    }

    public void EnforceStat(EEnforce enforce, float enforceFigure)
    {
        switch (enforce)
        {
            case EEnforce.STAFF_MINING_POWER:
            case EEnforce.HEAD_MINING_POWER:
                _finalMiningPower = Mathf.RoundToInt((_minerEntity.MiningPower + (int)enforceFigure + (_haveEquip ? _equip.FinalMiningPower : 0)) * Snack.MiningPowerBuff);
                break;
            case EEnforce.STAFF_MINING_SPEED:
            case EEnforce.HEAD_MINING_SPEED:
                _finalMiningSpeed = _minerEntity.MiningSpeed * (enforceFigure + (_haveEquip ? _equip.FinalMiningSpeed : 0)) * Snack.MiningSpeedBuff;
                _animator.SetFloat("MineSpeed", _finalMiningSpeed);
                _mineAnimWait = new WaitForSeconds(_miningAnimTime / _finalMiningSpeed);
                _mineWait = new WaitForSeconds(_miningTime / _finalMiningSpeed);
                break;
            case EEnforce.STAFF_MOVING_SPEED:
            case EEnforce.HEAD_MOVING_SPEED:
                _finalMovingSpeed = _minerEntity.MovingSpeed * (enforceFigure + (_haveEquip ? _equip.FinalMovingSpeed : 0)) * Snack.MovingSpeedBuff;
                _aiPath.maxSpeed = _finalMovingSpeed;
                break;
            case EEnforce.HEAD_CRITICAL_PERCENT:
                _finalCriticalPercent = _minerEntity.CriticalPercent + enforceFigure + (_haveEquip ? _equip.FinalCriticalPercent : 0);
                break;
            case EEnforce.HEAD_CRITICAL_POWER:
                _finalCriticalPower = _minerEntity.CriticalPower + enforceFigure + (_haveEquip ? _equip.FinalCriticalPower : 0);
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
        _fsm.StopState();
        if (_targetOre != null)
        {
            _targetOre.SetMiner(null);
            _targetOre = null;
        }
        _target = null;
    }

    public void SetCartTarget()
    {
        SetTartget(_cart.gameObject);
    }

    public void SetOreTarget(Ore targetOre)
    {
        SetTartget(targetOre.gameObject);
        _targetOre = targetOre;
    }


    public void StopMoving()
    {
        _aiPath.destination = transform.position;
    }

    public void StrikeOre()
    {
        _targetOre.Hit((int)(_finalMiningPower * (CheckCritical() ? _finalCriticalPower : 1)));
    }

    public void MinusCurrentMiningCount()
    {
        --_crtMiningCount;
    }

    public void SubmitMineral()
    {
        _player.SubmitMineral(_mineralCount);
        _cart.PlusMoney(_price);
        _mineralCount = 0;
        _price = 0;
        _crtMiningCount = _finalMiningCount;
        OnChangeMineralCount(_mineralCount);
    }

    public void EnterMine()
    {
        Debug.Log("EnterMine Begin");
        _fsm.StopState();
        _aiPath.destination = transform.parent.position;
        transform.position = transform.parent.position;
        if (_targetOre != null)
        {
            _targetOre.SetMiner(null);
            _targetOre = null;
        }
        _target = null;
        _crtMiningCount = _finalMiningCount;
        StartCoroutine(RestartWork());
    }

    public void SetAnim(string param)
    {
        InitAnimatorState();
        _animator.SetBool(param, true);
    }

    public void MoveToTarget()
    {
        _spriter.flipX = _target.transform.position.x < _rigid.position.x;
        _aiPath.destination = _target.transform.position;
    }

    private void Awake()
    {
        _spriter = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _aiPath = GetComponent<AIPath>();
        _fsm = GetComponent<MinerStateMachine>();
        _cart = GameManager.Cart;
        _player = GameManager.Player;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isEqualTarget = _target == collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_targetOre != null && _targetOre.gameObject == collision.gameObject)
        {
            _targetOre = null;
        }
    }

    private void InitAnimatorState()
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Idle", false);
        _animator.SetBool("Mine", false);
    }

    private bool CheckCritical()
    {
        return _finalCriticalPercent > Random.value;
    }
    private void SetTartget(GameObject target)
    {
        _isEqualTarget = false;
        _target = target;
    }

    private IEnumerator RestartWork()
    {
        yield return new WaitForSeconds(0.1f);
        _fsm.PlayState();
        _fsm.ChangeState(EMinerState.IDLE);
    }
}
