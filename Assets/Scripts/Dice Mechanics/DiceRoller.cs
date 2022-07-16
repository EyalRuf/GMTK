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
        rb = GetComponent<Rigidbody>();
        text = GetComponentInChildren<TMPro.TextMeshPro>();
        RandomDice();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            RollDice();
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
