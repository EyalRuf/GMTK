public class EnemyStateFactory : BaseStateMachineFactory
{
    public EnemyStateFactory(BaseStateMachine context) : base(context)
    {
        Pursue = new PursueState(context, this);
    }

    public PursueState Pursue { get; set; }
}
