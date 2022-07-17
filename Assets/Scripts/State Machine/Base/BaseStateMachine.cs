using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    [SerializeField, Tooltip("Tickrate of update behaviour in miniseconds")]
    public float Tickrate = 1500;
    public WaitForSeconds TickRateSeconds { get => _TickRateSeconds; }
    private WaitForSeconds _TickRateSeconds;

    public BaseState CurrentState { get; set; }
    public Coroutine CurrentStateCR { get; set; }

    protected virtual void Awake()
    {        
        // Set Attributes
        Tickrate /= 1000;
        _TickRateSeconds = new WaitForSeconds(Tickrate);
    }
    protected virtual void Start()
    {
        // Initialize First State
        CurrentState.EnterState();
        CurrentStateCR = StartCoroutine(CurrentState.BehaviourUpdateTick());
    }
    
    public void SwitchState(BaseState newState)
    {
        CurrentState.ExitState();

        if (CurrentStateCR != null)
        {
            StopCoroutine(CurrentStateCR);
            CurrentStateCR = null;
        }

        CurrentState = newState;
        newState.EnterState();

        CurrentStateCR = StartCoroutine(CurrentState.BehaviourUpdateTick());
    }

    protected void OnDisable()
    {
        StopAllCoroutines();
        if (CurrentStateCR != null)
            StopCoroutine(CurrentStateCR);
    }
}
