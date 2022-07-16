using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    float sqrBombRadius = 2f;

    [SerializeField]
    float expolsionForce = 2f;
    public void Detonate()
    {
        DiceRoller[] dice = Object.FindObjectsOfType<DiceRoller>();

        Vector3 position = transform.position;
        
        foreach (DiceRoller die in dice) {

            Vector3 offset = position - die.transform.position;
            offset.y = 0;

            
            if (Vector3.SqrMagnitude(offset) < sqrBombRadius) {
                Rigidbody dieRb = die.GetComponent<Rigidbody>();
                DiceRoller diceRoller = die.GetComponent<DiceRoller>();
                dieRb.AddExplosionForce(expolsionForce, position, 0);
                diceRoller.RollDice();
                print("HIT");
            }
        }

        Destroy(gameObject);
        
        Debug.Log("Boom");
    }
}
