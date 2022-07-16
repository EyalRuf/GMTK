using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    
    public int maxHealth;
    public int currentHealth;

    public int maxSpeed;
    public int currentSpeed;

    public int damage;

    public int maxDiceNumber;
    public int currentDiceNumber;

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
