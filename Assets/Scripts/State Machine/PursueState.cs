using UnityEngine;

public class PursueState : BaseState
{
    public PursueState(BaseStateMachine ctx, BaseStateMachineFactory factory) : base(ctx, factory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Started Pursue State!");
    }

    public override void ExitState()
    {

    }

    public override void UpdateBehaviour()
    {

    }
}
