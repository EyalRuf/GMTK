using UnityEngine;

public class Player : MonoBehaviour
{
    public DiceState CurrentDiceState { get; private set; } = DiceState.One;

    // Just for debugging
    public TMPro.TextMeshPro text;

    private void Start() => RandomDice();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = RayShooter.ShootRayToMousePosition();
            Vector3 pos = new(hit.point.x, transform.position.y, hit.point.z);
            transform.position = pos;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomDice();
        }
    }

    private void RandomDice()
    {
        int RandomInt = Random.Range(1, 7);
        DiceState rdmDice = (DiceState)RandomInt;
        UpdateDiceState(rdmDice);
    }
    private void UpdateDiceState(DiceState newDiceValue)
    {
        CurrentDiceState = newDiceValue;
        text.SetText(((int)newDiceValue).ToString());
    }
}
