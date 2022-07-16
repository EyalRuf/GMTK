using UnityEngine;
using System.Collections;

public abstract class BaseState
{
    protected Coroutine switchStateCR;
    protected EnemyStateMachine _ctx;
    protected EnemyStateFactory _factory;
    protected BaseState(EnemyStateMachine ctx, EnemyStateFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    public abstract void EnterState();

    public abstract void UpdateBehaviour();

    public abstract void ExitState();

    public IEnumerator BehaviourUpdateTick()
    {
        while (true)
        {
            UpdateBehaviour();
            yield return _ctx.TickRateSeconds;
        }
    }
}
