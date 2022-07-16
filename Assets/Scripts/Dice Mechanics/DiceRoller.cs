using System.Collections;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    #region Properties
    protected Rigidbody rb;
    private TMPro.TextMeshPro text;
    private Coroutine rollerCR;
    public DiceState CurrentDiceState { get; set; } = DiceState.One;
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
    /// <param name="explosionForce">The strength of the explosion at its core. Should decrease the further away the agent is from the source</param>
    public void ExplosionDiceRoll(Vector3 sourcePosition, float explosionForce)
    {
        if (rollerCR == null)
            rollerCR = StartCoroutine(DiceRoll(sourcePosition, explosionForce));
    }
    private IEnumerator DiceRoll(Vector3 bombSourcePos, float force)
    {
        PreDiceRoll();

        rb.AddExplosionForce(force, bombSourcePos, 2.5f);

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
        DiceState rdmDice = (DiceState)RandomInt;
        UpdateDiceState(rdmDice);
    }
    private void UpdateDiceState(DiceState newDiceValue)
    {
        CurrentDiceState = newDiceValue;
        text.SetText(((int)newDiceValue).ToString());
    }
    #endregion
}
