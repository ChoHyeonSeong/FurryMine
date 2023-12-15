using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopState : MinerState
{
    public StopState(MinerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter(Miner miner)
    {
        miner.SetAnim("Idle");
    }
}
