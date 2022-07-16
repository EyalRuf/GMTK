using UnityEngine;
using UnityEngine.AI;

public class RagdollTest : MonoBehaviour
{
    private Rigidbody rb;
    public float force = 100f;
    private void Start() => rb = GetComponent<Rigidbody>();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("boink");
            GetComponent<EnemyStateMachine>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<NavMeshAgent>().updateUpAxis = false;
            GetComponent<NavMeshAgent>().enabled = false;

            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            rb.AddTorque(transform.forward * force, ForceMode.Impulse);
        }
    }
}
