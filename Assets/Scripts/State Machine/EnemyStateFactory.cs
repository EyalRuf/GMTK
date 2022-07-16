public class EnemyStateFactory : BaseStateMachineFactory
{
    public EnemyStateFactory(EnemyStateMachine context) : base(context)
    {
        Pursue = new PursueState(context, this);
    }

    public PursueState Pursue { get; set; }
}
