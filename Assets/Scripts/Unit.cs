using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour {
    
    public int maxHealth;
    public int currentHealth;
    [SerializeField]
    private float invincibleLength = 2f;

    private bool invincible = false;

    public virtual void Damage(int amount) {
        if (!invincible)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                Death();
            }

            MakeInvincible(invincibleLength);
        }
    }

    public void Heal(int amount) {
        currentHealth += amount;
    }

    private void MakeInvincible(float amountOfSeconds) {
        _ = StartCoroutine(InvincibleTimer());

        IEnumerator InvincibleTimer() {
            invincible = true;
            yield return new WaitForSeconds(amountOfSeconds);
            invincible = false;
        }
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
