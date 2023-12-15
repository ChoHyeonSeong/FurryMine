using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum EMinerState
{
    IDLE,
    MOVE,
    THINK,
    MINE,
    SUBMIT,
    STOP
}

public class MinerStateMachine : MonoBehaviour
{
    private Miner _miner;
    private MinerState _idleState;
    private MinerState _moveState;
    private MinerState _thinkState;
    private MinerState _mineState;
    private MinerState _submitState;
    private MinerState _stopState;

    private MinerState _crtState;
    private bool _isStop;

    public void ChangeState(EMinerState state)
    {
        if (_isStop)
            return;

        if (_crtState != null)
            _crtState.Exit(_miner);
        Debug.Log($"{_miner.MinerName}-{state} Begin");
        switch (state)
        {
            case EMinerState.IDLE:
                _crtState = _idleState;
                break;
            case EMinerState.MOVE:
                _crtState = _moveState;
                break;
            case EMinerState.THINK:
                _crtState = _thinkState;
                break;
            case EMinerState.MINE:
                _crtState = _mineState;
                break;
            case EMinerState.SUBMIT:
                _crtState = _submitState;
                break;
        }
        _crtState.Enter(_miner);
    }

    public void StopState()
    {
        _crtState.Stop(_miner);
        _crtState = _stopState;
        _crtState.Enter(_miner);
        _isStop = true;
    }

    public void PlayState()
    {
        _isStop = false;
    }

    private void Awake()
    {
        _miner = GetComponent<Miner>();
        _idleState = new IdleState(this);
        _moveState = new MoveState(this);
        _thinkState = new ThinkState(this);
        _mineState = new MineState(this);
        _submitState = new SubmitState(this);
        _stopState = new StopState(this);
    }

    private void Update()
    {
        if (!_isStop)
        {
            _crtState.Execute(_miner);
        }
    }
}
