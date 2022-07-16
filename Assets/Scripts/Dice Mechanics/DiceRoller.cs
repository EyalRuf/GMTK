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
    private Rigidbody diceRb;
    [SerializeField]
    private Transform diceTransform;
    private Vector3 hoverOffset;  // The height the dice hovers with above the platform

    [SerializeField]
    private NumberRotation[] rotations;

    private bool rollCooldown = false;
    #endregion

    public virtual void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        hoverOffset = diceTransform.localPosition;
        RandomDice();
    }

    private void Update()
    {
        // What's currently up?
        if (Input.GetKeyDown(KeyCode.Space))
            RollDice();
    }

    public DiceStates GetNumber()
    {
        float closestDistance = Mathf.Infinity;
        Vector3 upside = diceTransform.position + Vector3.up;
        DiceStates state = CurrentDiceState;

        float forward = Vector3.Distance(diceTransform.forward + diceTransform.position, upside);
        if (forward < closestDistance)
        {
            closestDistance = forward;
            state = DiceStates.One;
            PlayerController.setSpeed(6);
        }

        float back = Vector3.Distance(-diceTransform.forward + diceTransform.position, upside);
        if (back < closestDistance)
        {
            closestDistance = back;
            state = DiceStates.Two;
            PlayerController.setSpeed(5.5f);
        }

        float left = Vector3.Distance(-diceTransform.right + diceTransform.position, upside);
        if (left < closestDistance)
        {
            closestDistance = left;
            state = DiceStates.Three;
            PlayerController.setSpeed(5);
        }

        float right = Vector3.Distance(diceTransform.right + diceTransform.position, upside);
        if (right < closestDistance)
        {
            closestDistance = right;
            state = DiceStates.Four;
            PlayerController.setSpeed(4.5f);
        }

        float bottom = Vector3.Distance(-diceTransform.up + diceTransform.position, upside);
        if (bottom < closestDistance)
        {
            closestDistance = bottom;
            state = DiceStates.Five;
            PlayerController.setSpeed(4);
        }

        float up = Vector3.Distance(diceTransform.up + diceTransform.position, upside);
        if (up < closestDistance)
        {
            state = DiceStates.Six;
            PlayerController.setSpeed(3.5f);
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

        //diceRb.useGravity = true;
        diceRb.AddTorque(Random.rotation.eulerAngles * force, ForceMode.Impulse);
        diceRb.angularDrag = 2.25f;

        yield return new WaitForSeconds(2f);

        //diceRb.useGravity = false;
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
                _ = StartCoroutine(LerpToHoverPosition(0.2f));
                //diceTransform.localRotation = targetRot;
            }
        }

        _ = StartCoroutine(DiceRollCooldown());
        PostDiceRoll();

        //_ = StartCoroutine(LerpToHoverPosition(0.5f));
        print("New Number is : " + number);

        // Go back to hover position with new number as the current number.
        IEnumerator LerpToHoverPosition(float speed)
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

        //diceRb.useGravity = true;
        Vector3 direction = (transform.position - bombSourcePos).normalized;

        rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
        rb.AddForce(direction * horizontalForce, ForceMode.Impulse);
        diceRb.AddTorque(Random.rotation.eulerAngles * rotationForce, ForceMode.Impulse);
        diceRb.angularDrag = 2.25f;

        yield return new WaitForSeconds(2f);

        //diceRb.useGravity = false;
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
                _ = StartCoroutine(LerpToHoverPosition(0.2f));
                //diceTransform.localRotation = targetRot;
            }
        }

        _ = StartCoroutine(DiceRollCooldown());
        PostDiceRoll();

        //_ = StartCoroutine(LerpToHoverPosition(0.5f));
        print("New Number is : " + number);

        // Go back to hover position with new number as the current number.
        IEnumerator LerpToHoverPosition(float speed)
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
    }
    #endregion
}

[System.Serializable]
public class NumberRotation
{
    public int number;
    public Vector3 rotation;
}
