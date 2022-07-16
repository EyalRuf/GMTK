using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DiceRoller : MonoBehaviour
{
    #region Properties
    private Rigidbody rb;
    private NavMeshAgent agent;
    private TMPro.TextMeshPro text;
    private Coroutine rollerCR;
    public DiceState CurrentDiceState { get; set; } = DiceState.One;
    private EnemyStateMachine esm;
    public float force = 3f;

    [SerializeField]
    private float rollCooldownTimer = 2f;
    private bool rollCooldown = false;
    #endregion

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;

        rb = GetComponent<Rigidbody>();
        esm = GetComponent<EnemyStateMachine>();
        text = GetComponentInChildren<TMPro.TextMeshPro>();
        RandomDice();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            RollDice();
    }

    public void RollDice()
    {
        if (rollerCR == null && !rollCooldown)
            rollerCR = StartCoroutine(DiceRoll());
    }

    private IEnumerator DiceRoll()
    {
        esm.enabled = false;
        rb.isKinematic = false;
        agent.enabled = false;

        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        rb.AddTorque(Random.rotation.eulerAngles * force, ForceMode.Impulse);

        yield return new WaitForSeconds(0.25f);
        yield return new WaitUntil(() => rb.angularVelocity.magnitude < 0.05f);

        rb.isKinematic = true;
        agent.enabled = true;
        esm.enabled = true;

        _ = StartCoroutine(DiceRollCooldown());

        rollerCR = default;
    }
    private IEnumerator DiceRollCooldown()
    {
        rollCooldown = true;
        yield return new WaitForSeconds(rollCooldownTimer);
        rollCooldown = false;
    }
    
        #region Internal
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
