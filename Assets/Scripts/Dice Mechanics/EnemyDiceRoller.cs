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
}
