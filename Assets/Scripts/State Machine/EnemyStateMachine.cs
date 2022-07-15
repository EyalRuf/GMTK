public class EnemyStateMachine : BaseStateMachine
{
    private EnemyStateFactory StatesFactory;
    protected override void Awake()
    {
        base.Awake();

        StatesFactory = new EnemyStateFactory(this);
        CurrentState = StatesFactory.Pursue;
    }
}
