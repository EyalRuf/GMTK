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
    public void SetToRandomRotation()
    {
        int randomNr = Random.Range(0, rotations.Length);
        CurrentDiceState = (DiceStates)randomNr;
        GetAndSetSpeed();
        Quaternion targetRot = Quaternion.Euler(rotations[randomNr].rotation);
        diceTransform.localRotation = targetRot;
    }
    public void GetAndSetSpeed()
    {
        switch (CurrentDiceState)
        {
            case DiceStates.One:
                ChangeSpeed(speedOn1);
                return;
            case DiceStates.Two:
                ChangeSpeed(speedOn2);
                return;
            case DiceStates.Three:
                ChangeSpeed(speedOn3);
                return;
            case DiceStates.Four:
                ChangeSpeed(speedOn4);
                return;
            case DiceStates.Five:
                ChangeSpeed(speedOn5);
                return;
            case DiceStates.Six:
                ChangeSpeed(speedOn6);
                return;
            default:
                ChangeSpeed(speedOn3);
                return; ;
        }
    }
    public override void ChangeSpeed(float newSpeed) => agent.speed = newSpeed;

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

