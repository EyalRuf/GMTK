using UnityEngine;
using System.Collections;

public abstract class BaseState
{
    protected Coroutine switchStateCR;
    protected BaseStateMachine _ctx;
    protected BaseStateMachineFactory _factory;
    protected BaseState(BaseStateMachine ctx, BaseStateMachineFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }
    /// <summary>
    /// Called once upon entering the state.
    /// </summary>
    /// <param name="manager">Reference to the Statemanager</param>
    public abstract void EnterState();

    /// <summary>
    /// Called on every tickrate
    /// </summary>
    /// <param name="manager">Reference to the Statemanager</param>
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
