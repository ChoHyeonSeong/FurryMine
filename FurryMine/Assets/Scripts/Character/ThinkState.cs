using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinkState : MinerState
{
    public ThinkState(MinerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter(Miner miner)
    {
        // ChangeState로 넘어오기 전에, CollisionExit가 일어날까?
        // 아마도 아닐거라 생각하지만, 일단 시험해보기
        if (miner.TargetOre == null)
        {
            miner.MinusCurrentMiningCount();
            _fsm.ChangeState(EMinerState.IDLE);
        }
        else
        {
            _fsm.ChangeState(EMinerState.MINE);
        }
    }
}
