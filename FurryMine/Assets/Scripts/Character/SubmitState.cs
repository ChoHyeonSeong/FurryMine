
using TMPro;

public class SubmitState : MinerState
{
    public SubmitState(MinerStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter(Miner miner)
    {
        miner.SubmitMineral();
        _fsm.ChangeState(EMinerState.IDLE);
    }
}
