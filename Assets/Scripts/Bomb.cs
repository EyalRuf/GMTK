using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    float sqrBombRadius = 2f;

    [SerializeField]
    float expolsionForce = 2f;

    private float upForce = 3f;
    [SerializeField]
    private float horizontalForce = 3f;
    [SerializeField]
    private float rotationForce = 3f;

    private float height;
    Ray ray;
    MeshRenderer renda;

    private void Start() {
        renda = GetComponent<MeshRenderer>();
        height = renda.bounds.size.y;
    }

    private void Update()
    {

        if (Physics.Raycast(transform.position, Vector3.down, height) || Physics.Raycast(transform.position, Vector3.up, height))
        { //Is grunded
            print(gameObject.name);
            //this.transform.eulerAngles = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = false;
            
        }

        transform.Find("bombring").Rotate(new Vector3(0, 0, 180 * Time.deltaTime)) ;
        if (Input.GetKeyDown(KeyCode.F))
        {
            Detonate();
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
