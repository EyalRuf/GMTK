using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour {
    
    public int maxHealth;
    public int currentHealth;
    [SerializeField]
    private float invincibleLength = 2f;

    private bool invincible = false;

    [Header("Invincibility Animation")]
    public float BlinkDuration = 0.15f;
    public Material InvincibleMaterial;

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

        StartCoroutine(PlayInvincibilityAnimation());
        
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

    IEnumerator PlayInvincibilityAnimation()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        
        // Get original materials
        Material[] originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalMaterials[i] = renderers[i].material;

        // Play blinking animation as long as invincible
        while (invincible)
        {
            foreach (var renderer in renderers)
                renderer.material = InvincibleMaterial;

            yield return new WaitForSeconds(BlinkDuration);

            for (int i = 0; i < renderers.Length; i++)
                renderers[i].material = originalMaterials[i];
            
            yield return new WaitForSeconds(BlinkDuration);
        }
    }
}
