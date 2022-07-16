using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RagdollTest : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;
    private EnemyStateMachine esm;
    public float force = 100f;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;

        rb = GetComponent<Rigidbody>();
        esm = GetComponent<EnemyStateMachine>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            esm.enabled = false;
            rb.isKinematic = false;
            agent.enabled = false;

            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            rb.AddTorque(Random.rotation.eulerAngles * force, ForceMode.Impulse);
            
            _ = StartCoroutine(WaitForLanded());
        }
    }

    private IEnumerator WaitForLanded()
    {
        yield return new WaitForSeconds(0.25f);
        yield return new WaitUntil(() => rb.angularVelocity.magnitude < 0.05f);

        rb.isKinematic = true;
        agent.enabled = true;
        esm.enabled = true;
    }
}
