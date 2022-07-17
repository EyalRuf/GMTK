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

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    Detonate();
        //}
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
