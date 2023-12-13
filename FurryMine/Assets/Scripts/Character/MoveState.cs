

public class MoveState : MinerState
{
    public MoveState(MinerStateMachine stateMachine) : base(stateMachine)
    {
    }


    public override void Enter(Miner miner)
    {
        // 애니메이션 재생
        miner.SetAnim("Run");
        miner.MoveToTarget();
    }

    public override void Execute(Miner miner)
    {
        if (miner.IsEqualTarget)
        {
            miner.StopMoving();
            if (miner.TargetOre == null)
            {
                _fsm.ChangeState(EMinerState.SUBMIT);
            }
            else
            {
                _fsm.ChangeState(EMinerState.MINE);
            }
        }
    }
}
