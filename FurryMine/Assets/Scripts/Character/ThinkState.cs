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
        // ChangeState�� �Ѿ���� ����, CollisionExit�� �Ͼ��?
        // �Ƹ��� �ƴҰŶ� ����������, �ϴ� �����غ���
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
