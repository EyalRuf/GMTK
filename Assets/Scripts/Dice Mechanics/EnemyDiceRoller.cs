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

