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

    private bool _haveEquip;
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
    private Coroutine _miningCoroutine;

    private void Awake()
    {
        _spriter = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _aiPath = GetComponent<AIPath>();
        _cart = GameManager.Cart;
    }

    public void Init(MinerEntity minerEntity, RuntimeAnimatorController animCtrl)
    {
        _animator.runtimeAnimatorController = animCtrl;
        _animator.SetBool("Idle", true);

        _minerEntity = minerEntity;

        _haveEquip = false;
        _isOreTarget = false;
        _price = 0;
        _mineralCount = 0;
        _target = null;
        _miningCoroutine = null;
        _finalMiningCount = _minerEntity.MiningCount;
        _crtMiningCount = _finalMiningCount;
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
        if (_isOreTarget)
        {
            _target.GetComponent<Ore>().SetMiner(null);
        }
        else
        {
            OnChangeMineralCount(0);
            GameManager.Cart.PlusMoney(_price);
        }
    }

    public void EnterMine()
    {
        _aiPath.destination = transform.parent.position;
        transform.position = transform.parent.position;
        SetAnim("Idle");
        if (_isOreTarget)
        {
            _target.GetComponent<Ore>().SetMiner(null);
            _isOreTarget = false;
        }
        if (_miningCoroutine != null)
        {
            StopCoroutine(_miningCoroutine);
            _miningCoroutine = null;
        }
        _target = null;
        _crtMiningCount = _finalMiningCount;
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
            _crtMiningCount = _finalMiningCount;
            OnChangeMineralCount(_mineralCount);
            StartCoroutine(BlockMove(_delayTime, null));
        }
    }

    private void StartMining(Ore targetOre)
    {
        Debug.Log("채광 시작");
        _aiPath.destination = transform.position;
        _miningCoroutine = StartCoroutine(MineOre(targetOre));
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
        // Hit -> 부셔지면 true 반환
        if (ore.Hit((int)(_finalMiningPower * (CheckCritical() ? _finalCriticalPower : 1))))
        {
            _isOreTarget = false;
            _crtMiningCount--;
            StartCoroutine(BlockMove(_delayTime, _crtMiningCount > 0 ? null : _cart.gameObject));
            _miningCoroutine = null;
        }
        else
        {
            _miningCoroutine = StartCoroutine(MineOre(ore));
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
