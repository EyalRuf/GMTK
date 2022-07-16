using UnityEngine;

public class PursueState : BaseState
{
    public PursueState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory) {}

    public override void EnterState()
    {
        //Debug.Log("Started Pursue State!");
    }

    public override void ExitState()
    {

    }

    public override void UpdateBehaviour()
    {
        _ctx.SetAgentDestination(_ctx.PlayerTransform.position);
    }
}
