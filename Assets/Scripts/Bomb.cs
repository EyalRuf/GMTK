using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    float sqrBombRadius = 2f;

    [SerializeField]
    float expolsionForce = 2f;
    [SerializeField]
    private float upForce = 3f;
    [SerializeField]
    private float horizontalForce = 3f;
    [SerializeField]
    private float rotationForce = 3f;

    public float height;
    Ray ray;
    MeshRenderer renda;

    private void Start() {
        renda = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            Detonate();
        }
        */
    }

    void OnTriggerStay(Collider other) {
        if (other.transform.tag == "Ground")
        {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.Find("BombRing").gameObject.SetActive(true);
            Debug.Log("hello");
        }
    }

    public void Detonate()
    {

        DiceRoller[] dice = Object.FindObjectsOfType<DiceRoller>();

        Vector3 position = transform.position;
        
        foreach (DiceRoller die in dice) {

            Vector3 offset = position - die.transform.position;
            offset.y = 0;

            if (Vector3.SqrMagnitude(offset) < sqrBombRadius)
            {
                die.ExplosionDiceRoll(transform.position, upForce, horizontalForce, rotationForce);
            }
        }

        Destroy(gameObject);
        Debug.Log("Boom");
    }
}
