using UnityEngine;
using UnityEngine.AI;

public class EnemyDiceRoller : DiceRoller
{
    #region Properties
    private NavMeshAgent agent;
    private EnemyStateMachine esm;
    #endregion

    public override void Start()
    {
        base.Start();
        esm = GetComponent<EnemyStateMachine>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            RollDice();
    }

    public override void ChangeSpeed(float newSpeed)
    {
        agent.speed = newSpeed;
    }

    public override void PreDiceRoll()
    {
        base.PreDiceRoll();
        rb.isKinematic = false;
        esm.enabled = false;
        agent.enabled = false;
    }
    public override void PostDiceRoll()
    {
        base.PostDiceRoll();
        rb.isKinematic = true;
        agent.enabled = true;
        esm.enabled = true;
    }

    public override DiceStates GetNumber()
    {
        float closestDistance = Mathf.Infinity;
        DiceStates state = CurrentDiceState;
        Vector3 upWorld = Vector3.up * 2;
        float bottom = Vector3.Distance(-diceTransform.up, upWorld);
        if (bottom < closestDistance)
        {
            closestDistance = bottom;
            state = DiceStates.One;
            ChangeSpeed(speedOn1);
        }

        float up = Vector3.Distance(diceTransform.up, upWorld);
        if (up < closestDistance)
        {
            closestDistance = up;
            state = DiceStates.Two;
            ChangeSpeed(speedOn2);
        }

        float left = Vector3.Distance(-diceTransform.right, upWorld);
        if (left < closestDistance)
        {
            closestDistance = left;
            state = DiceStates.Three;
            ChangeSpeed(speedOn3);
        }

        float right = Vector3.Distance(diceTransform.right, upWorld);
        if (right < closestDistance)
        {
            closestDistance = right;
            state = DiceStates.Four;
            ChangeSpeed(speedOn4);
        }

        float back = Vector3.Distance(-diceTransform.forward, upWorld);
        if (back < closestDistance)
        {
            closestDistance = back;
            state = DiceStates.Five;
            ChangeSpeed(speedOn5);
        }

        float forward = Vector3.Distance(diceTransform.forward, upWorld);
        if (forward < closestDistance)
        {
            closestDistance = forward;
            state = DiceStates.Six;
            ChangeSpeed(speedOn6);
        }
        return state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(diceTransform.up * 0.5f, 0.15f);  // 2
/*        Gizmos.DrawSphere(diceTransform.right * 0.5f, 0.15f);
        Gizmos.DrawSphere(diceTransform.forward * 0.5f, 0.15f);
        Gizmos.DrawSphere(-diceTransform.up * 0.5f, 0.15f);
        Gizmos.DrawSphere(-diceTransform.right * 0.5f, 0.15f);
        Gizmos.DrawSphere(-diceTransform.forward * 0.5f, 0.15f);*/

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(Vector3.up * 2, 0.2f);
    }
}

