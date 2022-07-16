using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStateMachine : BaseStateMachine
{
    private EnemyStateFactory StatesFactory;
    private NavMeshAgent agent;
    public DiceState CurrentDiceState { get; set; } = DiceState.One;
    public Transform PlayerTransform { get; private set; }

    [SerializeField]
    private TMPro.TextMeshPro text;

    protected override void Awake()
    {
        base.Awake();

        StatesFactory = new EnemyStateFactory(this);
        CurrentState = StatesFactory.Pursue;

        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        RandomDice();
    }

    /// <summary>
    /// Send the agent to a specified position
    /// </summary>
    /// <param name="destination">The destination's coordinates</param>
    public void SetAgentDestination(Vector3 destination)
    {
        if (!agent.pathPending)
        {
            agent.ResetPath();
            agent.SetDestination(destination);
        }
    }

    #region internal
    private void RandomDice()
    {
        int RandomInt = Random.Range(1, 7);
        DiceState rdmDice = (DiceState)RandomInt;
        UpdateDiceState(rdmDice);
    }
    private void UpdateDiceState(DiceState newDiceValue)
    {
        CurrentDiceState = newDiceValue;
        text.SetText(((int)newDiceValue).ToString());
    }
    #endregion
}
