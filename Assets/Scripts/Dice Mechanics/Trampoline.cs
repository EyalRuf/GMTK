using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DiceRoller diceRoller))
        {
            diceRoller.RollDice();
        }
    }
}
