using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maximumAmountOfHealth = 100;
    private bool invincible = false;

    public int CurrentAmountOfHealth { get; set; } = 100;

    private void Start()
    {
        CurrentAmountOfHealth = maximumAmountOfHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Collided with player");
        }
    }

    public void Damage(int amount)
    {
        if (!invincible)
            CurrentAmountOfHealth -= amount;
    }
    public void Heal(int amount)
    {
        CurrentAmountOfHealth += amount;
    }

    private void MakeInvincible(int amountOfSeconds)
    {
        _ = StartCoroutine(InvincibleTimer());

        IEnumerator InvincibleTimer()
        {
            invincible = true;
            yield return new WaitForSeconds(amountOfSeconds);
            invincible = false;
        }
    }
}
