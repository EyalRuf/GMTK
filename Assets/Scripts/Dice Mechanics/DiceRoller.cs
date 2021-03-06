using System.Collections;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    #region Properties
    protected Rigidbody rb;
    private Coroutine rollerCR;
    public DiceStates CurrentDiceState { get; set; } = DiceStates.One;
    public float standardForce = 3f;

    [SerializeField]
    private float rollCooldownTimer = 2f;
    [SerializeField]
    protected Rigidbody diceRb;
    [SerializeField]
    protected Transform diceTransform;
    private Vector3 hoverOffset;  // The height the dice hovers with above the platform

    [SerializeField]
    protected NumberRotation[] rotations;

    [SerializeField]
    private DiceStates startNumber = DiceStates.Three;

    private bool rollCooldown = false;

    [Space(5), Header("Speed on different numbers")]
    public float speedOn1 = 8.5f;
    public float speedOn2 = 8f;
    public float speedOn3 = 7.5f;
    public float speedOn4 = 6.5f;
    public float speedOn5 = 5.5f;
    public float speedOn6 = 4.5f;
    #endregion

    public virtual void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        hoverOffset = diceTransform.localPosition;
    }

    private void Start()
    {
        SetNumber(startNumber);
    }

    public void SetNumber(DiceStates number)
    {
        foreach (var rot in rotations)
        {
            if (rot.number == (int)number)
            {
                CurrentDiceState = number;
                Quaternion targetRot = Quaternion.Euler(rot.rotation);
                diceTransform.localRotation = targetRot;
            }
        }
    }

    public DiceStates GetNumber()
    {
        float closestDistance = Mathf.Infinity;
        DiceStates state = CurrentDiceState;

        float forward = Vector3.Distance(diceTransform.forward, Vector3.up);
        if (forward < closestDistance)
        {
            closestDistance = forward;
            state = DiceStates.One;
            ChangeSpeed(speedOn1);
        }

        float back = Vector3.Distance(-diceTransform.forward, Vector3.up);
        if (back < closestDistance)
        {
            closestDistance = back;
            state = DiceStates.Two;
            ChangeSpeed(speedOn2);
        }

        float left = Vector3.Distance(-diceTransform.right, Vector3.up);
        if (left < closestDistance)
        {
            closestDistance = left;
            state = DiceStates.Three;
            ChangeSpeed(speedOn3);
        }

        float right = Vector3.Distance(diceTransform.right, Vector3.up);
        if (right < closestDistance)
        {
            closestDistance = right;
            state = DiceStates.Four;
            ChangeSpeed(speedOn4);
        }

        float bottom = Vector3.Distance(-diceTransform.up, Vector3.up);
        if (bottom < closestDistance)
        {
            closestDistance = bottom;
            state = DiceStates.Five;
            ChangeSpeed(speedOn5);
        }

        float up = Vector3.Distance(diceTransform.up, Vector3.up);
        if (up < closestDistance)
        {
            closestDistance = up;
            state = DiceStates.Six;
            ChangeSpeed(speedOn6);
        }

        CurrentDiceState = state;

        return state;
    }

    public virtual void ChangeSpeed(float newSpeed)
    {
        PlayerController.setSpeed(newSpeed);
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
    public void RollDice(float force, float upForce)
    {
        if (rollerCR == null && !rollCooldown)
            rollerCR = StartCoroutine(DiceRoll(force, upForce));
    }

    private IEnumerator DiceRoll(float force)
    {
        PreDiceRoll();

        diceRb.AddTorque(Random.rotation.eulerAngles * force, ForceMode.Impulse);
        diceRb.angularDrag = 2.25f;

        yield return new WaitForSeconds(2f);

        diceRb.angularDrag = 7f;

        yield return new WaitUntil(() => diceRb.angularVelocity.magnitude <= 0.05f);

        diceRb.velocity = Vector3.zero;
        diceRb.angularVelocity = Vector3.zero;

        diceTransform.localPosition = hoverOffset;
        Quaternion targetRot;

        int number = (int)GetNumber();
        foreach (var rot in rotations)
        {
            if (rot.number == number)
            {
                targetRot = Quaternion.Euler(rot.rotation);
                _ = StartCoroutine(LerpToHoverPosition(0.2f, targetRot));
                //diceTransform.localRotation = targetRot;
            }
        }

        _ = StartCoroutine(DiceRollCooldown());
        PostDiceRoll();
    }
    private IEnumerator DiceRoll(float force, float upForce)
    {
        PreDiceRoll();

        diceRb.AddTorque(Random.rotation.eulerAngles * force, ForceMode.Impulse);
        rb.AddForce(transform.up * upForce, ForceMode.Impulse);
        diceRb.angularDrag = 2.25f;

        yield return new WaitForSeconds(2f);

        diceRb.angularDrag = 7f;

        yield return new WaitUntil(() => diceRb.angularVelocity.magnitude <= 0.05f);

        diceRb.velocity = Vector3.zero;
        diceRb.angularVelocity = Vector3.zero;

        diceTransform.localPosition = hoverOffset;
        Quaternion targetRot;

        int number = (int)GetNumber();
        foreach (var rot in rotations)
        {
            if (rot.number == number)
            {
                targetRot = Quaternion.Euler(rot.rotation);
                _ = StartCoroutine(LerpToHoverPosition(0.2f, targetRot));
                //diceTransform.localRotation = targetRot;
            }
        }

        _ = StartCoroutine(DiceRollCooldown());
        PostDiceRoll();

        print("New Number is : " + number);
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
        diceRb.AddTorque(Random.rotation.eulerAngles * rotationForce, ForceMode.Impulse);
        diceRb.angularDrag = 2.25f;

        yield return new WaitForSeconds(2f);

        diceRb.angularDrag = 7f;

        yield return new WaitUntil(() => diceRb.angularVelocity.magnitude <= 0.05f);

        diceRb.velocity = Vector3.zero;
        diceRb.angularVelocity = Vector3.zero;

        diceTransform.localPosition = hoverOffset;
        Quaternion targetRot;

        int number = (int)GetNumber();
        foreach (var rot in rotations)
        {
            if (rot.number == number)
            {
                targetRot = Quaternion.Euler(rot.rotation);
                _ = StartCoroutine(LerpToHoverPosition(0.2f, targetRot));
                //diceTransform.localRotation = targetRot;
            }
        }

        _ = StartCoroutine(DiceRollCooldown());
        PostDiceRoll();

        print("New Number is : " + number);
    }

    private IEnumerator LerpToHoverPosition(float speed, Quaternion targetRot)
    {
        float startTime = Time.time;
        Quaternion startRot = diceTransform.localRotation;

        float progress = 0f;
        while (progress <= 1f)
        {
            float timeSinceStarted = Time.time - startTime;
            progress = timeSinceStarted / speed;
            diceTransform.localRotation = Quaternion.Lerp(startRot, targetRot, progress);

            yield return new WaitForFixedUpdate();
        }

        //print("Done");
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

    #region Internal
    private void UpdateDiceState(DiceStates newDiceValue) => CurrentDiceState = newDiceValue;
    #endregion
}

[System.Serializable]
public class NumberRotation
{
    public int number;
    public Vector3 rotation;
}
