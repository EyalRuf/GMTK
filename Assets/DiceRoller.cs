using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DiceRoller : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;
    private EnemyStateMachine esm;
    public float force = 3f;
    public float cooldownTimer = 2f;
    private Coroutine rollerCR;
    private bool cooldown = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;

        rb = GetComponent<Rigidbody>();
        esm = GetComponent<EnemyStateMachine>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            RollDice();
    }

    public void RollDice()
    {
        if (rollerCR == null && !cooldown)
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
        cooldown = true;
        yield return new WaitForSeconds(cooldownTimer);
        cooldown = false;
    }
}
