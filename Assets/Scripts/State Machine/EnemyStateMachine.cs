using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Health))]
public class EnemyStateMachine : BaseStateMachine
{
    #region Properties
    private EnemyStateFactory StatesFactory;
    private NavMeshAgent agent;
    public Transform PlayerTransform { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        StatesFactory = new EnemyStateFactory(this);
        CurrentState = StatesFactory.Pursue;

        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable() => base.Start();  // Sets the Agent to pursue the player again.

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
