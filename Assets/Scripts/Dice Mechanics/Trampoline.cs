using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float rollForce = 5f;
    [SerializeField] private float upForce = 5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DiceRoller diceRoller))
        {
            diceRoller.RollDice(rollForce, upForce);
        }
    }
}
