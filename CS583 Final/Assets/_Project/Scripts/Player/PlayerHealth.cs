using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;      // Max hearts

    // Backing field
    private int currentHealth;

    // For the UI and other scripts
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

    // Change health when damage is taken
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

        // Die if health is 0 or less
        if (CurrentHealth <= 0)
            Die();
    }

    // Called by the heart pickup
    public bool Heal(int amount)
    {
        if (amount <= 0) return false;

        // Already full
        if (CurrentHealth >= maxHealth)
            return false;

        CurrentHealth += amount;
        return true;
    }

    void Die()
    {
        // In case death while paused
        Time.timeScale = 1f;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
