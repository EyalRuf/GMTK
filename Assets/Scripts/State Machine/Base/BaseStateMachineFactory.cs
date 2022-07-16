public abstract class BaseStateMachineFactory
{
    protected BaseStateMachine _context;

    protected BaseStateMachineFactory(BaseStateMachine context) => _context = context;
}
