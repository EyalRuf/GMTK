using UnityEngine;

public class Bomb : MonoBehaviour
{
    public void Detonate()
    {
        DiceRoller[] dice = Object.FindObjectsOfType<DiceRoller>();

        Vector3 position = transform.position;
        
        foreach (DiceRoller die in dice) {

            Vector3 offset = position - die.transform.position;
            offset.y = 0;

            const float sqrBombRadius = 2f;
            if (Vector3.SqrMagnitude(offset) < sqrBombRadius)
            {
                // TODO: Throw away die
            }
        }

        Destroy(gameObject);
        
        Debug.Log("Boom");
    }
}
