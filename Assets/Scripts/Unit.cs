using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour {
    
    public int maxHealth;
    public int currentHealth;

    private bool invincible = false;

    public bool Damage(int amount) {
        if (!invincible)
            currentHealth -= amount;

        if (currentHealth <= 0)
            return true;
        else 
            return false;
    }

    public void Heal(int amount) {
        currentHealth += amount;
    }

    private void MakeInvincible(int amountOfSeconds) {
        _ = StartCoroutine(InvincibleTimer());

        IEnumerator InvincibleTimer() {
            invincible = true;
            yield return new WaitForSeconds(amountOfSeconds);
            invincible = false;
        }
    }

}
