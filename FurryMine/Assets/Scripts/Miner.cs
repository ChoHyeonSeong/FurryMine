using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Miner : MonoBehaviour
{
    [SerializeField]
    private Ore _targetOre;

    private bool _isMining;
    private int _power = 10;
    private float _mineSpeed = 1;
    private WaitForSeconds _mineAnimWait = new WaitForSeconds(0.333f);
    private float _moveSpeed = 1;
    private string _oreTag = "Ore";

    private SpriteRenderer _spriter;
    private Animator _animator;
    private Rigidbody2D _rigid;
    private WaitForSeconds _mineWait;
    private void Awake()
    {
        _spriter = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _mineWait = new WaitForSeconds(_mineSpeed);
        _animator.SetBool("Idle", true);
        _isMining = false;
    }

    private void FixedUpdate()
    {
        if (_targetOre != null && !_isMining)
        {
            _spriter.flipX = _targetOre.RigidPosition.x < _rigid.position.x;
            Vector2 dirPos = _targetOre.RigidPosition - _rigid.position;
            Vector2 nextPos = dirPos.normalized * Time.fixedDeltaTime * _moveSpeed;
            _rigid.MovePosition(nextPos + _rigid.position);
            _rigid.velocity = Vector2.zero;
            SetAnim("Run");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(_oreTag))
            return;
        StartMining(collision.gameObject.GetComponent<Ore>());
    }

    private void StartMining(Ore targetOre)
    {
        _isMining = true;
        _targetOre = targetOre;
        StartCoroutine(Mine());
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

    private IEnumerator Mine()
    {
        SetAnim("Idle");
        yield return _mineWait;
        SetAnim("Mine");
        yield return _mineAnimWait;
        if (_targetOre.Hit(_power))
        {
            SetAnim("Idle");
            _isMining = false;
            _targetOre = null;
        }
        else
        {
            StartCoroutine(Mine());
        }
    }
}
