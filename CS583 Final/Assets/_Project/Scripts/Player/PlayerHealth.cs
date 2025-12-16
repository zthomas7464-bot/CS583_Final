using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; // Max hearts

    private int currentHealth;

    public int CurrentHealth
    {
        get { return currentHealth; }
        private set { currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    void Start()
    {
        // Set health at start
        CurrentHealth = maxHealth;
    }

    // change health when damage is taken
    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        CurrentHealth -= amount;

        // Trigger damage flash
        HealthBarUI ui = FindObjectOfType<HealthBarUI>();
        if (ui != null)
        {
            ui.TriggerDamageFlash();
        }

        // die if health is 0 or less
        if (CurrentHealth <= 0)
            Die();
    }

    // Called by heart pickup
    public bool Heal(int amount)
    {
        if (amount <= 0) return false;

        if (CurrentHealth >= maxHealth)
            return false;

        CurrentHealth += amount;
        return true;
    }

    public void ResetHealth()
    {
        CurrentHealth = maxHealth;
    }

    void Die()
    {
        RespawnManager rm = FindObjectOfType<RespawnManager>();
        if (rm != null)
        {
            rm.RespawnPlayer();
        }
        else
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
    }
}
