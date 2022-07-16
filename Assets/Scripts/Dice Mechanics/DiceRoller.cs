using System.Collections;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    #region Properties
    protected Rigidbody rb;
    private TMPro.TextMeshPro text;
    private Coroutine rollerCR;
    public DiceStates CurrentDiceState { get; set; } = DiceStates.One;
    public float standardForce = 3f;

    [SerializeField]
    private float rollCooldownTimer = 2f;
    private bool rollCooldown = false;
    #endregion

    public virtual void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        text = GetComponentInChildren<TMPro.TextMeshPro>();
        RandomDice();
    }

    private void Update()
    {
        // What's currently up?
        // Which direction is closest to Vector3.up?
        if (Input.GetKeyDown(KeyCode.Space))
            RollDice();
    }

    private DiceStates GetNumber()
    {
        float closestDistance = Mathf.Infinity;
        Vector3 upside = transform.position + Vector3.up;
        DiceStates state = CurrentDiceState;

        float forward = Vector3.Distance(transform.forward + transform.position, upside);
        if (forward < closestDistance)
        {
            closestDistance = forward;
            state = DiceStates.One;
        }

        float back = Vector3.Distance(-transform.forward + transform.position, upside);
        if (back < closestDistance)
        {
            closestDistance = back;
            state = DiceStates.Two;
        }

        float left = Vector3.Distance(-transform.right + transform.position, upside);
        if (left < closestDistance)
        {
            closestDistance = left;
            state = DiceStates.Three;
        }

        float right = Vector3.Distance(transform.right + transform.position, upside);
        if (right < closestDistance)
        {
            closestDistance = right;
            state = DiceStates.Four;
        }

        float bottom = Vector3.Distance(-transform.up + transform.position, upside);
        if (bottom < closestDistance)
        {
            closestDistance = bottom;
            state = DiceStates.Five;
        }

        float up = Vector3.Distance(transform.up + transform.position, upside);
        if (up < closestDistance)
        {
            state = DiceStates.Six;
        }

        return state;
    }

    public void RollDice()
    {
        if (rollerCR == null && !rollCooldown)
            rollerCR = StartCoroutine(DiceRoll(standardForce));
    }
    public void RollDice(float force)
    {
        if (rollerCR == null && !rollCooldown)
            rollerCR = StartCoroutine(DiceRoll(force));
    }

    private IEnumerator DiceRoll(float force)
    {
        PreDiceRoll();

        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        rb.AddTorque(Random.rotation.eulerAngles * force, ForceMode.Impulse);

        yield return new WaitForSeconds(0.25f);
        yield return new WaitUntil(() => rb.angularVelocity.magnitude < 0.05f);
        
        _ = StartCoroutine(DiceRollCooldown());

        PostDiceRoll();
    }

    /// <summary>
    /// A diceroll as a consequence of an explosion at the sourcePosition.
    /// </summary>
    /// <param name="sourcePosition">The position of the center of the explosion</param>
    /// <param name="upForce">The strength with which the dice will be thrown in the air</param>
    /// <param name="horizontalForce">The strength with which the dice will be pushed in the opposite direction of the explosion</param>
    /// <param name="rotationForce">The strength with which the dice will be pushed in terms of rotation</param>
    public void ExplosionDiceRoll(Vector3 sourcePosition, float upForce, float horizontalForce, float rotationForce)
    {
        if (rollerCR == null)
            rollerCR = StartCoroutine(DiceRoll(sourcePosition, upForce, horizontalForce, rotationForce));
    }
    private IEnumerator DiceRoll(Vector3 bombSourcePos, float upForce, float horizontalForce, float rotationForce)
    {
        PreDiceRoll();

        Vector3 direction = (transform.position - bombSourcePos).normalized;

        rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
        rb.AddForce(direction * horizontalForce, ForceMode.Impulse);
        rb.AddTorque(direction * rotationForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.25f);
        yield return new WaitUntil(() => rb.angularVelocity.magnitude < 0.05f);

        _ = StartCoroutine(DiceRollCooldown());

        PostDiceRoll();
    }
    public virtual void PreDiceRoll()
    {
    }
    public virtual void PostDiceRoll()
    {
        rollerCR = default;
    }
    private IEnumerator DiceRollCooldown()
    {
        rollCooldown = true;
        yield return new WaitForSeconds(rollCooldownTimer);
        rollCooldown = false;
    }

    public static Vector3 GetForward(DiceStates currentDiceState)
    {
        switch (currentDiceState)
        {
            case DiceStates.One:
                return Vector3.forward;

            case DiceStates.Two:
                return Vector3.back;

            case DiceStates.Three:
                return Vector3.left;

            case DiceStates.Four:
                return Vector3.right;

            case DiceStates.Five:
                return Vector3.down;

            case DiceStates.Six:
                return Vector3.up;

            default:
                return Vector3.forward;
        }
    }
    #region Internal
    private void RandomDice()
    {
        int RandomInt = Random.Range(1, 7);
        DiceStates rdmDice = (DiceStates)RandomInt;
        UpdateDiceState(rdmDice);
    }
    private void UpdateDiceState(DiceStates newDiceValue)
    {
        CurrentDiceState = newDiceValue;
        text.SetText(((int)newDiceValue).ToString());
    }
    #endregion
}
