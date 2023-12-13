public class MinerState 
{
    protected MinerStateMachine _fsm;
    public MinerState(MinerStateMachine stateMachine) { _fsm = stateMachine; }
    public virtual void Enter(Miner miner) { }
    public virtual void Execute(Miner miner) { }
    public virtual void Exit(Miner miner) { }
    public virtual void Stop(Miner miner) { }
}
