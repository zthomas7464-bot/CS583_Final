using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Image[] heartImages;
    public Image damageFlashImage;

    [Header("Damage Flash Settings")]
    public float flashDuration = 0.4f;
    [Range(0f,1f)]
    public float maxFlashAlpha = 0.35f;


    [Header("Low Health Settings")]
    public bool tintHeartsOnLowHealth = true;
    public Color lowHealthColor = new Color(0.54f, 0.01f, 0.01f);
    [Range(0f, 1f)]
    public float lowHealthThreshold = 0.33f;

    private float flashTimer = 0f;
    private Color[] originalHeartColors;

    // Start is called before the first frame update
    void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }

        if (playerHealth == null)
        {
            Debug.LogError("HealthBarUI: Reference to PlayerHealth is missing");
            enabled = false;
            return;
        }

        //Store original heart color
        if (heartImages != null && heartImages.Length > 0)
        {
            originalHeartColors = new Color[heartImages.Length];
            for (int i = 0; i < heartImages.Length; i++)
            {
                if (heartImages[i] != null)
                {
                    originalHeartColors[i] = heartImages[i].color;
                }
            }
        }

        //Make sure damage flash starts invisible
        if (damageFlashImage != null)
        {
            Color c = damageFlashImage.color;
            c.a = 0f;
            damageFlashImage.color = c;
        }
        
        //update the HP Text
        UpdateHearts();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth == null) return;

        //update the Hearts
        UpdateHearts();


        //Damage flash fade out
        if (damageFlashImage != null && flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;

            float t = 1f - (flashTimer / flashDuration);
            t = Mathf.Clamp01(t);

            //Smooth from maxFlashAlpha to 0
            float alpha = Mathf.SmoothStep(maxFlashAlpha, 0f, t);

            Color c = damageFlashImage.color;
            c.a = alpha;
            damageFlashImage.color = c;
        }
    }

    void UpdateHearts()
    {
        if (heartImages == null || heartImages.Length == 0) return;

        int currentHealth = Mathf.Clamp(playerHealth.CurrentHealth, 0, heartImages.Length);
        int maxHealth = playerHealth.maxHealth;

        //Show or hide hearts based on health
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] == null) continue;

            //Make heart visible if the index is less than health
            heartImages[i].enabled = (i < currentHealth);
        }

        //Tint the heart on low health
        if (tintHeartsOnLowHealth && originalHeartColors != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            bool isLow = healthPercent <= lowHealthThreshold;

            for (int i = 0; i < heartImages.Length; i++)
            {
                if (heartImages[i] == null) continue;

                if (!heartImages[i].enabled) continue;

                heartImages[i].color = isLow ? lowHealthColor : originalHeartColors[i];
            }
        }
    }

    public void TriggerDamageFlash()
    {
        if (damageFlashImage == null)
            return;

        flashTimer = flashDuration;

        Color c = damageFlashImage.color;
        c.a = maxFlashAlpha;
        damageFlashImage.color = c;
    }
}
