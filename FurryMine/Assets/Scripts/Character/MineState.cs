using System.Collections;
using System.Collections.Generic;
using System.Data.Odbc;
using UnityEngine;

public class MineState : MinerState
{
    private Coroutine _waitMining;
    public MineState(MinerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter(Miner miner)
    {
        _waitMining = miner.StartCoroutine(WaitMining(miner));
    }
    public override void Stop(Miner miner)
    {
        miner.StopCoroutine(_waitMining);
    }

    private IEnumerator WaitMining(Miner miner)
    {
        miner.SetAnim("Idle");
        yield return miner.MineWait;
        miner.SetAnim("Mine");
        yield return miner.MineAnimWait;
        miner.StrikeOre();
        _fsm.ChangeState(EMinerState.THINK);
    }
}
