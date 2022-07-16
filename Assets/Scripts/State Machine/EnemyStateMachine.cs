using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStateMachine : BaseStateMachine
{
    private EnemyStateFactory StatesFactory;
    private NavMeshAgent agent;
    public Transform PlayerTransform { get; private set; }


    [SerializeField, Tooltip("The amount of seconds between each update when pursuing a target.\n" +
        "A lower value means a higher performance cost!")] 
    private float followCRInterval = 0.3f;

    protected override void Awake()
    {
        base.Awake();

        StatesFactory = new EnemyStateFactory(this);
        CurrentState = StatesFactory.Pursue;

        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
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
}
