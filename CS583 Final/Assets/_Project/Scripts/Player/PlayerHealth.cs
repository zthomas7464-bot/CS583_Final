using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3; // Max hearts
    private int currentHealth;

    [Header("Audio")]
    public AudioClip damageSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    public int CurrentHealth
    {
        get { return currentHealth; }
        private set { currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    void Start()
    {
        // Set health at start
        CurrentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("PlayerHealth: No Audio Source Found for PlayerHealth");
        else
        {
            Debug.Log("PlayerHealth: AudioSource found on Player object.");
            if (damageSound == null)
                Debug.LogWarning("PlayerHealth: damageSound is NOT assigned in the Inspector.");
            if (deathSound == null)
                Debug.LogWarning("PlayerHealth: deathSound is NOT assigned in the Inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("PlayerHealth: H pressed, testing TakeDamage(1).");
            TakeDamage(1);
        }
    }

    // change health when damage is taken
    public void TakeDamage(int amount)
    {
        Debug.Log($"PlayerHealth: TakeDamage called with amount = {amount}");

        if (amount <= 0)
        {
            Debug.Log("PlayerHealth: amount <= 0, returning without doing anything.");
            return;
        }

        CurrentHealth -= amount;
        Debug.Log($"PlayerHealth: CurrentHealth is now {CurrentHealth}");

        if (audioSource != null && damageSound != null)
        {
            Debug.Log("PlayerHealth: Playing damageSound.");
            audioSource.PlayOneShot(damageSound, 1.5f);
        }
        else
        {
            if (audioSource == null)
                Debug.LogWarning("PlayerHealth: Cannot play damageSound, audioSource is null.");
            if (damageSound == null)
                Debug.LogWarning("PlayerHealth: Cannot play damageSound, damageSound clip is null.");
        }

        // Trigger damage flash
        HealthBarUI ui = FindObjectOfType<HealthBarUI>();
        if (ui != null)
            ui.TriggerDamageFlash();
        else
            Debug.LogWarning("PlayerHealth: No HealthBarUI found in scene to trigger flash.");

        // die if health is 0 or less
        if (CurrentHealth <= 0)
        {
            Debug.Log("PlayerHealth: CurrentHealth <= 0, calling Die().");
            Die();
        }
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
        Debug.Log("PlayerHealth: Die() called.");

        if (audioSource != null && deathSound != null)
        {
            Debug.Log("PlayerHealth: Playing deathSound.");
            audioSource.PlayOneShot(deathSound, 2f);
        }
        else
        {
            if (audioSource == null)
                Debug.LogWarning("PlayerHealth: Cannot play deathSound, audioSource is null.");
            if (deathSound == null)
                Debug.LogWarning("PlayerHealth: Cannot play deathSound, deathSound clip is null.");
        }

        RespawnManager rm = FindObjectOfType<RespawnManager>();
        if (rm != null)
        {
            Debug.Log("PlayerHealth: RespawnManager found, calling RespawnPlayer().");
            rm.RespawnPlayer();
        }
        else
        {
            Debug.Log("PlayerHealth: No RespawnManager found, reloading scene.");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
    }
}