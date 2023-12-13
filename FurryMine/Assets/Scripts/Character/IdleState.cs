using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MinerState
{
    private WaitForSeconds _idleWaitTime = new WaitForSeconds(0.3f);
    private bool _isMovable;
    private Coroutine _waitIdling;

    public IdleState(MinerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter(Miner miner)
    {
        // 애니메이션 재생
        miner.SetAnim("Idle");
        // 잠깐 유휴상태
        _waitIdling = miner.StartCoroutine(WaitIdling());
    }

    public override void Execute(Miner miner)
    {
        if(_isMovable)
        {
            if(miner.CurrentMiningCount > 0)
            {
                Ore ore = Miner.RequestOre();
                if (ore != null)
                {
                    miner.SetOreTarget(ore);
                    ore.SetMiner(miner);
                    _fsm.ChangeState(EMinerState.MOVE);
                }
            }
            else
            {
                miner.SetCartTarget();
                _fsm.ChangeState(EMinerState.MOVE);
            }
        }
    }

    public override void Stop(Miner miner)
    {
        miner.StopCoroutine(_waitIdling);
    }

    private IEnumerator WaitIdling()
    {
        _isMovable = false;
        yield return _idleWaitTime;
        _isMovable = true;
    }
}
